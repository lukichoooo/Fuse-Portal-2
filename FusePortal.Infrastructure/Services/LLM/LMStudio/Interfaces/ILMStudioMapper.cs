using FusePortal.Application.UseCases.Academic.Exams;
using FusePortal.Application.UseCases.Convo.Chats;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces
{
    public interface ILMStudioMapper
    {
        MessageLLMDto ToMessageDto(
                LMStudioResponse response,
                Guid chatId);

        string ToOutputText(LMStudioResponse response);

        LMStudioRequest ToRequest(
                MessageLLMDto msg,
                string? previousResponseId = null,
                string? rulesPrompt = null);

        LMStudioRequest ToRequest(
                string text,
                string? rulesPrompt = null);

        LMStudioRequest ToRequest(
                ExamDto examDto,
                string? rulesPrompt = null);


        LMStudioCompletionRequest ToCompletionRequest(
            string text,
            string? rulesPrompt = null,
            string? jsonSchema = null);

    }
}
