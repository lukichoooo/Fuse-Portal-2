using FusePortal.Application.UseCases.Convo.Chats;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces
{
    public interface ILMStudioMapper
    {
        MessageLLMDto ToMessageDto(
                LMStudioResponse response,
                Guid chatId);

        LMStudioRequest ToRequest(
                MessageLLMDto msg,
                string? previousResponseId = null,
                string? rulesPrompt = null);

        LMStudioRequest ToRequest(string text,
                string? rulesPrompt = null);

        string ToOutputText(LMStudioResponse response);

        // LMStudioRequest ToRequest(ExamDto examDto, string? rulesPrompt = null);
    }
}
