using System.Net;
using System.Text.Json;
using AutoFixture;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Exceptions;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Implementation;
using FusePortal.Infrastructure.Settings.LLM;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace InfrastructureTests.LLMTests.LMStudio
{
    [TestFixture]
    public class LMStudioApiTests
    {
        private Fixture _fix;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<ILogger<LMStudioApi>> _loggerMock;
        private JsonSerializerOptions _serializerOptions;
        private LLMApiSettings _apiSettings;
        private LLMApiSettingKeys _settingKeys;

        [SetUp]
        public void Setup()
        {
            _fix = new Fixture();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

            _loggerMock = new Mock<ILogger<LMStudioApi>>();

            _apiSettings = _fix.Build<LLMApiSettings>()
                                .With(x => x.URL, "http://localhost:1234")
                                .With(x => x.ChatRoute, "/v1/chat/completions")
                                .Create();
            _settingKeys = _fix.Create<LLMApiSettingKeys>();

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        private LMStudioApi CreateSut(
                ILLMApiResponseStreamer? responseStreamer = null)
        {
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            responseStreamer ??= new Mock<ILLMApiResponseStreamer>().Object;

            return new(
                _loggerMock.Object,
                _serializerOptions,
                httpClient,
                responseStreamer
            );
        }



        [Test]
        public async Task SendMessageAsync_Success()
        {
            var request = _fix.Create<LMStudioRequest>();
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
            var sut = CreateSut();

            var result = await sut.SendMessageAsync(request, _apiSettings);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedResponse.Id));

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    CheckContentIsSnakeCase(req.Content, request)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public void SendMessageAsync_WhenApiReturnsError_ShouldLogAndThrowException()
        {
            var request = _fix.Create<LMStudioRequest>();
            const string errorContent = "Internal Server Error";

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(errorContent)
                });
            var sut = CreateSut();

            var ex = Assert.ThrowsAsync<LMStudioApiException>(async () =>
                await sut.SendMessageAsync(request, _apiSettings));

            Assert.That(ex.Message, Does.Contain(errorContent));

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!
                        .Contains("LMStudio returned")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void SendMessageAsync_WhenResponseIsSuccessButNull_ShouldThrowException()
        {
            CreateSut();
            var request = _fix.Create<LMStudioRequest>();

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
                    Content = new StringContent("null")
                });
            var sut = CreateSut();

            Assert.ThrowsAsync<LMStudioApiException>(async () =>
                await sut.SendMessageAsync(request, _apiSettings));
        }


        [Test]
        public async Task SendMessageStreamingAsyncAsync_Success()
        {
            var request = _fix.Create<LMStudioRequest>();
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

            bool onRecievedCalled = false;
            Func<string, Task> onRecieved = (string _) =>
            {
                onRecievedCalled = true;
                return Task.CompletedTask;
            };
            var streamerMock = new Mock<ILLMApiResponseStreamer>();
            streamerMock.Setup(s => s.ReadResponseAsStreamAsync(
                        It.IsAny<HttpResponseMessage>(),
                        onRecieved
                        ))
                .ReturnsAsync((HttpResponseMessage _, Func<string, Task> onRecieved) =>
                        {
                            onRecieved.Invoke("message");
                            return expectedResponse;
                        });
            var sut = CreateSut(streamerMock.Object);

            var result = await sut.SendMessageWithStreamingAsync(
                    request,
                    _apiSettings,
                    onRecieved);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedResponse.Id));
            Assert.That(onRecievedCalled, Is.True);

            _httpMessageHandlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        CheckContentIsSnakeCase(req.Content, request)
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }


        [Test]
        public async Task SendMessageStreamingAsyncAsync_CompletedNull_Throws()
        {
            var request = _fix.Create<LMStudioRequest>();
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

            bool onRecievedCalled = false;
            Func<string, Task> onRecieved = (string _) =>
            {
                onRecievedCalled = true;
                return Task.CompletedTask;
            };

            var streamerMock = new Mock<ILLMApiResponseStreamer>();
            streamerMock.Setup(s => s.ReadResponseAsStreamAsync(
                        It.IsAny<HttpResponseMessage>(),
                        onRecieved
                        ))
                .ReturnsAsync((HttpResponseMessage _, Func<string, Task> onRecieved) =>
                        {
                            onRecieved.Invoke("message");
                            return null;
                        });
            var sut = CreateSut(streamerMock.Object);

            Assert.ThrowsAsync<LMStudioApiException>(async () =>
                    await sut.SendMessageWithStreamingAsync(
                    request,
                    _apiSettings,
                    onRecieved));

            _httpMessageHandlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        CheckContentIsSnakeCase(req.Content, request)
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }


        [Test]
        public async Task SendMessageStreamingAsyncAsync_UnsuccessfulResponse_Throws()
        {
            var request = _fix.Create<LMStudioRequest>();
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
                    StatusCode = HttpStatusCode.InternalServerError,
                });

            var sut = CreateSut();

            Assert.ThrowsAsync<LMStudioApiException>(async () =>
                    await sut.SendMessageWithStreamingAsync(
                    request,
                    _apiSettings,
                    null));

            _httpMessageHandlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        CheckContentIsSnakeCase(req.Content, request)
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }




        // Helper

        private static bool CheckContentIsSnakeCase(HttpContent? content, LMStudioRequest originalRequest)
        {
            if (content == null) return false;

            var jsonString = content.ReadAsStringAsync().Result;

            bool hasSnakeCaseKey = jsonString.Contains("\"previous_response_id\"");
            bool hasOriginalValue = jsonString.Contains(originalRequest.PreviousResponseId ?? "");

            return hasSnakeCaseKey && hasOriginalValue;
        }


    }
}
