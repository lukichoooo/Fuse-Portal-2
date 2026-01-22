using AutoFixture;
using FusePortal.Application.Interfaces.Services;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Infrastructure.Services.LLM.Implementation;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Chat;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Implementation;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;
using FusePortal.Infrastructure.Settings.LLM;
using Microsoft.Extensions.Options;
using Moq;

namespace InfrastructureTests.LLMTests.LMStudio
{
    [TestFixture]
    public class LMStudioMessageServiceTests
    {
        private readonly LLMApiSettingKeys _settingKeys = new()
        {
            Chat = "chat",
            Parser = "parsar"
        };

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


        private readonly Fixture _fix = new();
        private LMStudioMapper _mapper;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _fix.Behaviors.Remove(new OmitOnRecursionBehavior());

            var inputGenMock = new Mock<ILLMInputGenerator>();
            inputGenMock.Setup(g => g.GenerateInput(It.IsAny<MessageLLMDto>(), It.IsAny<string>()))
                .Returns("INPUT");
            var keyOptions = Options.Create(_settingKeys);

            var settingsChooserMock = new Mock<ILLMApiSettingsChooser>();
            settingsChooserMock.Setup(s => s.GetChatSettings())
                .Returns(_apiSettings);
            settingsChooserMock.Setup(s => s.GetParserSettings())
                .Returns(_apiSettings);
            settingsChooserMock.Setup(s => s.GetExamServiceSettings())
                .Returns(_apiSettings);


            _mapper = new LMStudioMapper(
                    inputGenMock.Object,
                    settingsChooserMock.Object
                    );

        }

        private ILLMMessageService CreateSut(ILMStudioApi api, IChatMetadataService metadataService)
        {
            var settingKeys = _fix.Create<LLMApiSettingKeys>();
            var keyOptionsMock = new Mock<IOptions<LLMApiSettingKeys>>();
            keyOptionsMock.Setup(x => x.Value)
                .Returns(settingKeys);

            var apiOptionsMonitorMock = new Mock<IOptionsMonitor<LLMApiSettings>>();
            apiOptionsMonitorMock.Setup(m => m.Get(It.IsAny<string>()))
                .Returns(_apiSettings);

            var promptProviderMock = new Mock<IFileReader>();

            var chooser = new LLMApiSettingsChooser(
                keyOptionsMock.Object,
                apiOptionsMonitorMock.Object,
                promptProviderMock.Object
            );

            return new LMStudioMessageService(
                    api,
                    _mapper,
                    metadataService,
                    chooser);
        }

        [Test]
        public async Task SendMessageAsync_Success()
        {
            var msg = _fix.Create<MessageLLMDto>();
            var request = _fix.Create<LMStudioRequest>();
            var response = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                .ReturnsAsync(response);

            var dataServiceMock = new Mock<IChatMetadataService>();
            dataServiceMock.Setup(a => a.GetLastResponseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync("id");
            dataServiceMock.Setup(a => a.SetLastResponseIdAsync(
                        It.IsAny<Guid>(), It.IsAny<string>()));

            var service = CreateSut(apiMock.Object, dataServiceMock.Object);

            var res = await service.SendMessageAsync(msg, default);

            Assert.That(res, Is.Not.Null);
            Assert.That(res.ChatId, Is.EqualTo(msg.ChatId));
            AssertServiceCalls(apiMock, dataServiceMock);
        }


        [Test]
        public async Task SendMessageAsync_NullLastResponseId_Success()
        {
            var msg = _fix.Create<MessageLLMDto>();
            var request = _fix.Create<LMStudioRequest>();
            var response = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings))
                .ReturnsAsync(response);

            var dataServiceMock = new Mock<IChatMetadataService>();
            dataServiceMock.Setup(a => a.GetLastResponseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);
            dataServiceMock.Setup(a => a.SetLastResponseIdAsync(
                        It.IsAny<Guid>(), It.IsAny<string>()));

            var service = CreateSut(apiMock.Object, dataServiceMock.Object);

            var res = await service.SendMessageAsync(msg, default);

            Assert.That(res, Is.Not.Null);
            Assert.That(res.ChatId, Is.EqualTo(msg.ChatId));
            AssertServiceCalls(apiMock, dataServiceMock);
        }



        [Test]
        public async Task SendMessageAsync_WithStreaming_Success()
        {
            var msg = _fix.Create<MessageLLMDto>();
            var request = _fix.Create<LMStudioRequest>();
            var action = _fix.Create<Func<string, Task>>();
            var response = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageWithStreamingAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings,
                        action))
                .ReturnsAsync(response);

            var dataServiceMock = new Mock<IChatMetadataService>();
            dataServiceMock.Setup(a => a.GetLastResponseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync("id");

            dataServiceMock.Setup(a => a.SetLastResponseIdAsync(
                        It.IsAny<Guid>(), It.IsAny<string>()));

            var service = CreateSut(apiMock.Object, dataServiceMock.Object);

            var res = await service.SendMessageStreamingAsync(msg, action, default);

            Assert.That(res, Is.Not.Null);
            Assert.That(res.ChatId, Is.EqualTo(msg.ChatId));
            AssertStreamServiceCalls(apiMock, dataServiceMock, action);
        }


        [Test]
        public async Task SendMessageAsync_WithStreaming_NullLastResponseId_Success()
        {
            var msg = _fix.Create<MessageLLMDto>();
            var request = _fix.Create<LMStudioRequest>();
            var action = _fix.Create<Func<string, Task>>();
            var response = _fix.Create<LMStudioResponse>();

            var apiMock = new Mock<ILMStudioApi>();
            apiMock.Setup(a => a.SendMessageWithStreamingAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings,
                        action))
                .ReturnsAsync(response);

            var dataServiceMock = new Mock<IChatMetadataService>();
            dataServiceMock.Setup(a => a.GetLastResponseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);
            dataServiceMock.Setup(a => a.SetLastResponseIdAsync(
                        It.IsAny<Guid>(), It.IsAny<string>()));

            var service = CreateSut(apiMock.Object, dataServiceMock.Object);

            var res = await service.SendMessageStreamingAsync(msg, action, default);

            Assert.That(res, Is.Not.Null);
            Assert.That(res.ChatId, Is.EqualTo(msg.ChatId));
            AssertStreamServiceCalls(apiMock, dataServiceMock, action);
        }

        private void AssertStreamServiceCalls(
                Mock<ILMStudioApi> apiMock,
                Mock<IChatMetadataService> dataServiceMock,
                Func<string, Task>? action = null)
        {
            apiMock.Verify(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings),
                        Times.Never());
            apiMock.Verify(a => a.SendMessageWithStreamingAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings,
                        action),
                        Times.Once());
            dataServiceMock.Verify(a => a.GetLastResponseIdAsync(It.IsAny<Guid>()),
                        Times.Once());
            dataServiceMock.Verify(a => a.SetLastResponseIdAsync(
                        It.IsAny<Guid>(), It.IsAny<string>()),
                        Times.Once());
        }


        private void AssertServiceCalls(
                Mock<ILMStudioApi> apiMock,
                Mock<IChatMetadataService> dataServiceMock)
        {
            apiMock.Verify(a => a.SendMessageAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings),
                        Times.Once());
            apiMock.Verify(a => a.SendMessageWithStreamingAsync(
                        It.IsAny<LMStudioRequest>(),
                        _apiSettings,
                        It.IsAny<Func<string, Task>>()),
                        Times.Never());
            dataServiceMock.Verify(a => a.GetLastResponseIdAsync(It.IsAny<Guid>()),
                        Times.Once());
            dataServiceMock.Verify(a => a.SetLastResponseIdAsync(
                        It.IsAny<Guid>(), It.IsAny<string>()),
                        Times.Once());
        }

    }
}
