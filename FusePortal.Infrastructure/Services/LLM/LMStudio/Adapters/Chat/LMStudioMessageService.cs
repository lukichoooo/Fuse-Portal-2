using FusePortal.Application.Interfaces.Services;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Chat
{
    public class LMStudioMessageService : ILLMMessageService
    {

        private readonly ILMStudioApi _api;
        private readonly ILMStudioMapper _mapper;
        private readonly IChatMetadataService _metadataService;
        private readonly ILLMApiSettingsChooser _apiSettings;

        public LMStudioMessageService(
                ILMStudioApi api,
                ILMStudioMapper mapper,
                IChatMetadataService metadataService,
                ILLMApiSettingsChooser apiSettings)
        {
            _api = api;
            _mapper = mapper;
            _metadataService = metadataService;
            _apiSettings = apiSettings;
        }

        public async Task<MessageLLMDto> SendMessageAsync(
                MessageLLMDto message,
                CancellationToken ct = default)
        {
            var chatId = message.ChatId;
            var lastId = await _metadataService.GetLastResponseIdAsync(message.ChatId);
            var request = _mapper.ToRequest(
                    message,
                    lastId,
                    _apiSettings.GetChatPrompt());

            request.Stream = false;
            LMStudioResponse response = await _api.SendMessageAsync(
                     request,
                     _apiSettings.GetChatSettings(),
                     ct);

            await _metadataService.SetLastResponseIdAsync(chatId, response.Id);
            return _mapper.ToMessageDto(response, chatId);
        }

        public async Task<MessageLLMDto> SendMessageStreamingAsync(
                MessageLLMDto message,
                Func<string, Task> onStreamReceived,
                CancellationToken ct = default)
        {
            var chatId = message.ChatId;
            var lastId = await _metadataService.GetLastResponseIdAsync(message.ChatId);
            var request = _mapper.ToRequest(
                    message,
                    lastId,
                    _apiSettings.GetChatPrompt());

            request.Stream = true;
            LMStudioResponse response = await _api.SendMessageWithStreamingAsync(
                     request,
                     _apiSettings.GetChatSettings(),
                     onStreamReceived,
                     ct);

            await _metadataService.SetLastResponseIdAsync(chatId, response.Id);
            return _mapper.ToMessageDto(response, chatId);
        }
    }
}
