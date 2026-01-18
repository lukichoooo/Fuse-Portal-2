using FusePortal.Application.UseCases.Convo.Chats;

namespace FusePortal.Application.Interfaces.Services
{
    public interface ILLMMessageService
    {
        Task<MessageLLMDto> SendMessageAsync(
                MessageLLMDto message,
                CancellationToken ct = default);

        Task<MessageLLMDto> SendMessageStreamingAsync(
                MessageLLMDto message,
                Func<string, Task>? onStreamReceived,
                CancellationToken ct = default);

    }
}
