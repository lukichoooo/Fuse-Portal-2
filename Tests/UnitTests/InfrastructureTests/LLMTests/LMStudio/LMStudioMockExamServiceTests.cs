using AutoFixture;
using FusePortal.Application.UseCases.Academic.Exams;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Infrastructure.Services.ExamGenerator;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;
using FusePortal.Infrastructure.Settings.LLM;
using Microsoft.Extensions.Options;
using Moq;

namespace InfrastructureTests.LLMTests.LMStudio
{
    [TestFixture]
    public class LMStudioMockExamServiceTests
    {
        private readonly Fixture _fix = new();

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _fix.Behaviors.Add(new OmitOnRecursionBehavior());
        }

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

        private LMStudioExamService CreateSut(
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

            return new(api, mapper, settingsChooserMock.Object);
        }

        [Test]
        public async Task GenerateExamAsync_Success()
        {
            // Arrange
            var apiResponse = _fix.Create<LMStudioResponse>();
            var expectedExamOutput = "MOCK EXAM\nSubject: Computer Science\n...";

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse))
                .Returns(expectedExamOutput);

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);

            var subject = new Subject(_fix.Create<string>(), _fix.Create<Guid>(), _fix.Create<string>());
            foreach (var s in subject.Syllabuses)
                subject.RemoveSyllabus(s.Id);

            var syllabusName = _fix.Create<string>();
            var syllabusContent = _fix.Create<string>();
            subject.AddSyllabus(syllabusName, syllabusContent);


            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            // Act
            var result = await sut.GenerateExamQuestionsAsync(subject);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expectedExamOutput));
        }



        [TestCase(0)]
        [TestCase(100)]
        [TestCase(10)]
        public async Task GetExamResultsAsync_Success(int score)
        {
            // Arrange
            string text = $"aujifdawifhjauif Score:{score}";
            var examDto = _fix.Create<ExamDto>();

            var apiResponse = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ReturnsAsync(apiResponse);

            var lmsRequest = _fix.Create<LMStudioRequest>();

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);
            mapperMock.Setup(m => m.ToOutputText(apiResponse))
                .Returns(text);

            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            var result = await sut.GradeExamAsync(examDto);

            Assert.That(result.scoreFrom100, Is.EqualTo(score));
        }

        [Test]
        public async Task GenerateExamAsync_CallsApiWithCorrectSettings()
        {
            // Arrange
            var apiResponse = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(It.IsAny<LMStudioResponse>()))
                .Returns(_fix.Create<string>());

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);

            var subject = new Subject(_fix.Create<string>(), _fix.Create<Guid>(), _fix.Create<string>());
            foreach (var s in subject.Syllabuses)
                subject.RemoveSyllabus(s.Id);

            var syllabusName = _fix.Create<string>();
            var syllabusContent = _fix.Create<string>();
            subject.AddSyllabus(syllabusName, syllabusContent);

            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            // Act
            await sut.GenerateExamQuestionsAsync(subject);

            // Assert
            apiMock.Verify(a => a.SendMessageAsync(
                It.IsAny<LMStudioRequest>(),
                _apiSettings), Times.Once);
        }


        [Test]
        public async Task GenerateExamAsync_CallsMapperToOutputTextWithApiResponse()
        {
            // Arrange
            var apiResponse = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse))
                .Returns(_fix.Create<string>());

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);

            var subject = new Subject(_fix.Create<string>(), _fix.Create<Guid>(), _fix.Create<string>());
            foreach (var s in subject.Syllabuses)
                subject.RemoveSyllabus(s.Id);

            var syllabusName = _fix.Create<string>();
            var syllabusContent = _fix.Create<string>();
            subject.AddSyllabus(syllabusName, syllabusContent);

            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            // Act
            await sut.GenerateExamQuestionsAsync(subject);

            // Assert
            mapperMock.Verify(m => m.ToOutputText(apiResponse), Times.Once);
        }

        [Test]
        public async Task GenerateExamAsync_ReturnsEmptyString_WhenMapperReturnsEmpty()
        {
            // Arrange
            var apiResponse = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ReturnsAsync(apiResponse);

            var mapperMock = new Mock<ILMStudioMapper>();
            mapperMock.Setup(m => m.ToOutputText(apiResponse))
                .Returns(string.Empty);

            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);

            var subject = new Subject(_fix.Create<string>(), _fix.Create<Guid>(), _fix.Create<string>());
            foreach (var s in subject.Syllabuses)
                subject.RemoveSyllabus(s.Id);

            var syllabusName = _fix.Create<string>();
            var syllabusContent = _fix.Create<string>();
            subject.AddSyllabus(syllabusName, syllabusContent);

            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            // Act
            var result = await sut.GenerateExamQuestionsAsync(subject);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GenerateExamAsync_ThrowsException_WhenApiThrows()
        {
            // Arrange
            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                    .ThrowsAsync(new Exception("API error"));

            var mapperMock = new Mock<ILMStudioMapper>();
            var lmsRequest = _fix.Create<LMStudioRequest>();
            mapperMock.Setup(m => m.ToRequest(
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .Returns(lmsRequest);

            var subject = new Subject(_fix.Create<string>(), _fix.Create<Guid>(), _fix.Create<string>());
            foreach (var s in subject.Syllabuses)
                subject.RemoveSyllabus(s.Id);

            var syllabusName = _fix.Create<string>();
            var syllabusContent = _fix.Create<string>();
            subject.AddSyllabus(syllabusName, syllabusContent);


            var sut = CreateSut(apiMock.Object, mapperMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await sut.GenerateExamQuestionsAsync(subject));
        }
    }
}
