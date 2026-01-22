using System.Text.Json;
using AutoFixture;
using AutoFixture.Kernel;
using FusePortal.Application.Interfaces.Services.PortalTransfer;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Domain.Common.ValueObjects.LectureDate;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Portal;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;
using FusePortal.Infrastructure.Settings.LLM;
using Microsoft.Extensions.Options;
using Moq;

namespace InfrastructureTests.LLMTests.LMStudio
{
    [TestFixture]
    public class LMStudioPortalParserTests
    {
        private readonly Fixture _fix = new();

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _fix.Behaviors.Add(new OmitOnRecursionBehavior());

            _fix.Customizations.Add(new LectureDateBuilder());
        }

        private record PortalLLMDto(List<SubjectLLMDto> Subjects);


        private readonly LLMApiSettings _apiSettings = new()
        {
            URL = "asdadjaod",
            ChatRoute = "/v1/chat/completions",

            Model = "qwen2.5-7b-instruct",
            TimeoutMins = 1,

            Temperature = 0.7f,
            MaxTokens = 2048,
            Stream = false
        };

        private LMStudioPortalTransferService CreateSut(
            ILMStudioApi api,
            ILMStudioMapper mapper)
        {
            LLMApiSettingKeys settingKeys = new()
            {
                Chat = "chatyy",
                Parser = "parserr",
                Exam = "exam-gen-key"
            };

            var keyOptions = Options.Create(settingKeys);

            var settingsChooserMock = new Mock<ILLMApiSettingsChooser>();
            settingsChooserMock.Setup(s => s.GetChatSettings())
                .Returns(_apiSettings);
            settingsChooserMock.Setup(s => s.GetParserSettings())
                .Returns(_apiSettings);
            settingsChooserMock.Setup(s => s.GetExamServiceSettings())
                .Returns(_apiSettings);

            return new(
                    api,
                    mapper,
                    settingsChooserMock.Object);
        }


        [Test]
        public async Task ParsePortalHtml_Success()
        {
            var apiResponse = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ReturnsAsync(apiResponse);

            var parserResponseDto = _fix.Create<PortalLLMDto>();
            var outputText = JsonSerializer.Serialize(parserResponseDto);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse))
                .Returns(outputText);
            var lmsRequest = _fix.Create<LMStudioRequest>();

            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<MessageLLMDto>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);

            var request = _fix.Create<string>();
            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            var res = await sut.SavePortalAsync(request);

            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.Not.Empty);
            Assert.That(res.Select(x => x.Name),
                    Is.EquivalentTo(parserResponseDto.Subjects.Select(x => x.Name)));
        }


        [Test]
        public async Task ParsePortalHtml_EmptyGrade_DoesNotThrow()
        {
            var apiResponse = _fix.Create<LMStudioResponse>();

            var brokenJson = @"{
            ""subjects"": [
                { ""name"": ""Test Subject"",
                    ""metadata"": ""meta"",
                    ""schedules"": [],
                    ""lecturers"": [],
                    ""syllabuses"": [] }
            ],
            ""metadata"": ""meta""
        }";

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(It.IsAny<LMStudioRequest>(), It.IsAny<LLMApiSettings>()))
                   .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse)).Returns(brokenJson);

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(It.IsAny<MessageLLMDto>(), It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(lmsRequest);

            var request = _fix.Create<string>();
            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            Assert.DoesNotThrowAsync(async () => await sut.SavePortalAsync(request));
        }

        [Test]
        public async Task ParsePortalHtml_ExtraFields_IgnoresExtras()
        {
            var apiResponse = _fix.Create<LMStudioResponse>();

            var extraJson = @"{
            ""subjects"": [
                { ""name"": ""Extra"", 
                    ""metadata"": ""meta"",
                    ""foo"": ""bar"",
                    ""schedules"": [],
                    ""lecturers"": [],
                    ""syllabuses"": [] }
            ],
            ""metadata"": ""meta"",
            ""extraField"": ""ignoreMe""
        }";

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(It.IsAny<LMStudioRequest>(), It.IsAny<LLMApiSettings>()))
                   .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse)).Returns(extraJson);

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(It.IsAny<MessageLLMDto>(), It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(lmsRequest);

            var request = _fix.Create<string>();
            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            var res = await sut.SavePortalAsync(request);

            Assert.That(res, Is.Not.Null);
            Assert.That(res[0].Name, Is.EqualTo("Extra"));
            Assert.That(res[0].Metadata, Is.EqualTo("meta"));
        }

        [Test]
        public async Task ParsePortalHtml_MissingOptionalFields_DoesNotThrow()
        {
            var apiResponse = _fix.Create<LMStudioResponse>();

            var missingFieldsJson = @"
            {
                ""subjects"": 
                [
                    {
                        ""name"": ""NoSchedules"",
                        ""metadata"": ""meta"" 
                    }
                ],
                ""metadata"": ""meta""
            }";

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(It.IsAny<LMStudioRequest>(), It.IsAny<LLMApiSettings>()))
                   .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse)).Returns(missingFieldsJson);

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(It.IsAny<MessageLLMDto>(), It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(lmsRequest);

            var request = _fix.Create<string>();
            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            Assert.DoesNotThrowAsync(async () => await sut.SavePortalAsync(request));
        }

    }



    public class LectureDateBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is not Type t || t != typeof(LectureDate))
                return new NoSpecimen();

            // Always ensure Start <= End
            var start = DateTime.UtcNow;
            var end = start.AddHours(1);

            return new LectureDate(start, end);
        }
    }

}
