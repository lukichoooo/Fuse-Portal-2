using FusePortal.Infrastructure.Services.LLM.LMStudio;

namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface ILLMApiResponseStreamReader
    {
        Task<LMStudioResponse?> ReadResponseAsStreamAsync(
                HttpResponseMessage responseMessage,
                Func<string, Task>? onReceived,
                CancellationToken ct = default);
    }
}
