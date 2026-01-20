using FusePortal.Infrastructure.Settings.LLM;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces
{
    public interface ILMStudioApi
    {
        Task<LMStudioResponse> SendMessageAsync(
                LMStudioRequest request,
                LLMApiSettings settings,
                CancellationToken ct = default);

        Task<LMStudioResponse> SendMessageWithStreamingAsync(
                LMStudioRequest request,
                LLMApiSettings settings,
                Func<string, Task>? action,
                CancellationToken ct = default);
    }
}
