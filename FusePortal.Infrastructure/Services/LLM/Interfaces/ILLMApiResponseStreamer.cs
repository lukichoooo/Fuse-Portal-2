using FusePortal.Infrastructure.Services.LLM.LMStudio;

namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface ILLMApiResponseStreamer
    {
        Task<LMStudioResponse?> ReadResponseAsStreamAsync(
                HttpResponseMessage responseMessage,
                Func<string, Task>? onReceived);
    }
}
