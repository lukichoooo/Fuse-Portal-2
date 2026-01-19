using System.Net;
using System.Text.Json;
using AutoFixture;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Application.Interfaces.Services;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Application.UseCases.Convo.Chats.Commands.CreateChat;
using FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Domain.SeedWork;
using FusePortal.Infrastructure.Data;
using FusePortal.Infrastructure.Repo;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Chat;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Implementation;
using FusePortal.Infrastructure.Settings.LLM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace IntergrationTests.ChatTests
{
    [TestFixture]
    public class CommandTests
    {
        private AppDbContext _context;
        private static readonly Fixture _fix = new();

        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();

        readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private readonly LLMApiSettings _apiSettings = new()
        {
            URL = "http://localhost:1234",
            ChatRoute = "/v1/chat/completions",

            Model = "qwen2.5-7b-instruct",
            TimeoutMins = 1,

            Temperature = 0.7f,
            MaxTokens = 2048,
            Stream = false
        };

        private readonly LLMApiSettingKeys _apiSettingKeys = new()
        {
            Parser = "parser",
            Chat = "chat",
        };

        private readonly LLMInputSettings _inputSettings = new()
        {
            UserInputDelimiter = "---USER INPUT---",
            FileNameDelimiter = "---FILE NAME---",
            FileContentDelimiter = "---FILE CONTENT---",
            RulesPromptDelimiter = "---RULES---"
        };

        [OneTimeSetUp]
        public void BeforeAll()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            var expectedResponse = _fix.Create<LMStudioResponse>();
            var responseJson = JsonSerializer.Serialize(expectedResponse, _serializerOptions);

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            _fix.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task BeforeEach()
        {
            _context.Universities.RemoveRange(_context.Universities.ToList());
            _context.Universities.RemoveRange(_context.Universities.ToList());
            _context.Users.RemoveRange(_context.Users.ToList());
            await _context.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task AfterAll()
        {
            await _context.DisposeAsync();
        }


        [Test]
        public async Task CreateChat_Success()
        {
            // Arrange
            var user = _fix.Create<User>();
            var userRepo = new UserRepo(_context);

            var chatRepo = new ChatRepo(_context);
            await userRepo.AddAsync(user);


            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new CreateChatCommandHandler(
                    chatRepo,
                    identityMock.Object,
                    uow);
            await _context.SaveChangesAsync();

            // Act
            string chatName = "MY New Chat";
            await sut.Handle(new CreateChatCommand(chatName), default);

            // Asset
            Assert.That(user.Chats, Has.Count.EqualTo(1));
            Assert.That(user.Chats.First().Name, Is.EqualTo(chatName));
            Assert.That(_context.Users.First().Chats.First().Name,
                    Is.EqualTo(chatName));
        }


        [Test]
        public async Task SendMessage_Success()
        {
            // Arrange
            var user = _fix.Create<User>();
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(user);

            var chat = new Chat("MyChat", user.Id);
            var chatRepo = new ChatRepo(_context);
            await chatRepo.AddAsync(chat);

            var fileRepo = new FileRepo(_context);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);


            var uow = new EfUnitOfWork(_context, CreateDispatcher());

            var llmService = new LMStudioMessageService(
                        CreateLMStudioApi(),
                        CreateLMStudioMapper(),
                        CreateMetadataService(),
                        CreateSettingsChooser()
                    );

            var sut = new SendMessageCommandHandler(
                    chatRepo,
                    identityMock.Object,
                    fileRepo,
                    llmService,
                    CreateMessageStreamer(),
                    uow);
            await _context.SaveChangesAsync();

            // Act
            string messageText = "my message text yay";
            await sut.Handle(new SendMessageCommand(
                        chat.Id,
                        messageText,
                        [],
                        Streaming: false
                        ), default);

            // Asset
            Assert.That(user.Chats, Has.Count.EqualTo(1));
            var messages = user.Chats.First().Messages;
            Assert.That(user.Chats.First().Messages, Is.EquivalentTo(messages));
            Assert.That(messages, Has.Count.EqualTo(2));
            Assert.That(messages.First().Text, Is.EqualTo(messageText));
        }



        [Test]
        public async Task SendMessage_Streaming_Success()
        {
            // Arrange
            var user = _fix.Create<User>();
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(user);

            var chat = new Chat("MyChat", user.Id);
            var chatRepo = new ChatRepo(_context);
            await chatRepo.AddAsync(chat);

            var fileRepo = new FileRepo(_context);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var uow = new EfUnitOfWork(_context, CreateDispatcher());

            var llmService = new LMStudioMessageService(
                        CreateLMStudioApi(),
                        CreateLMStudioMapper(),
                        CreateMetadataService(),
                        CreateSettingsChooser()
                    );

            var sut = new SendMessageCommandHandler(
                    chatRepo,
                    identityMock.Object,
                    fileRepo,
                    llmService,
                    CreateMessageStreamer(),
                    uow);
            await _context.SaveChangesAsync();

            // Act
            string messageText = "my message text yay";
            await sut.Handle(new SendMessageCommand(
                        chat.Id,
                        messageText,
                        [],
                        Streaming: true
                        ), default);

            // Asset
            Assert.That(user.Chats, Has.Count.EqualTo(1));
            var messages = user.Chats.First().Messages;
            Assert.That(user.Chats.First().Messages, Is.EquivalentTo(messages));
            Assert.That(messages, Has.Count.EqualTo(2));
            Assert.That(messages.First().Text, Is.EqualTo(messageText));
        }



        // Helper

        private IMessageStreamer CreateMessageStreamer()
        {
            var streamerMock = new Mock<IMessageStreamer>();
            streamerMock
                .Setup(s => s.StreamAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            return streamerMock.Object;
        }

        private ILLMApiResponseStreamReader CreateResponseStreamReader()
        {
            var streamerMock = new Mock<ILLMApiResponseStreamReader>();
            var response = _fix.Create<LMStudioResponse>();

            streamerMock
                .Setup(s => s.ReadResponseAsStreamAsync(
                    It.IsAny<HttpResponseMessage>(),
                    It.IsAny<Func<string, Task>>()))
                .ReturnsAsync(response);

            return streamerMock.Object;
        }

        private LMStudioApi CreateLMStudioApi()
        {
            Mock<ILogger<LMStudioApi>> _loggerMock;
            _loggerMock = new Mock<ILogger<LMStudioApi>>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            return new(
                _loggerMock.Object,
                _serializerOptions,
                httpClient,
                CreateResponseStreamReader()
            );
        }

        private LMStudioMapper CreateLMStudioMapper()
        {


            var generatorMock = new Mock<ILLMInputGenerator>();
            generatorMock.Setup(g => g.GenerateInput(It.IsAny<MessageLLMDto>(), It.IsAny<string>()))
                .Returns("INPUT");
            generatorMock.Setup(g => g.GenerateInput(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("INPUT");

            var keyOptions = Options.Create(_apiSettingKeys);
            var inputOptions = Options.Create(_inputSettings);

            return new LMStudioMapper(
                    generatorMock.Object,
                    CreateSettingsChooser());

        }

        private IDomainEventDispatcher CreateDispatcher()
        {
            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));
            return dispatcher.Object;
        }


        private IChatMetadataService CreateMetadataService()
        {

            var metadataMock = new Mock<IChatMetadataService>();
            metadataMock.Setup(m => m.GetLastResponseIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync("LastResponseId");
            metadataMock.Setup(m => m.SetLastResponseIdAsync(It.IsAny<Guid>(), It.IsAny<string>()));

            return metadataMock.Object;
        }


        private ILLMApiSettingsChooser CreateSettingsChooser()
        {

            var settingsChooserMock = new Mock<ILLMApiSettingsChooser>();
            settingsChooserMock.Setup(s => s.GetParserSettings())
                .Returns(_apiSettings);
            settingsChooserMock.Setup(s => s.GetChatSettings())
                .Returns(_apiSettings);
            settingsChooserMock.Setup(s => s.GetExamGeneratorSettings())
                .Returns(_apiSettings);

            return settingsChooserMock.Object;
        }

    }
}


