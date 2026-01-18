using FusePortal.Application.Interfaces.Services;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Chat
{
    public class LMStudioMessageService(
            ILMStudioApi api,
            ILMStudioMapper mapper,
            IChatMetadataService metadataService,
            ILLMApiSettingsChooser apiSettings) : ILLMMessageService
    {

        private readonly ILMStudioApi _api = api;
        private readonly ILMStudioMapper _mapper = mapper;
        private readonly IChatMetadataService _metadataService = metadataService;
        private readonly ILLMApiSettingsChooser _apiSettings = apiSettings;

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
                     _apiSettings.GetChatSettings());

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
                     onStreamReceived);

            await _metadataService.SetLastResponseIdAsync(chatId, response.Id);
            return _mapper.ToMessageDto(response, chatId);
        }
    }
}
