using FusePortal.Application.UseCases.Academic.Exams;
using FusePortal.Application.UseCases.Convo.Chats;

namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface ILLMInputGenerator
    {
        string GenerateInput(MessageLLMDto msg, string? rules);
        string GenerateInput(string text, string? rules);
        string GenerateInput(ExamDto exam, string? rules);
    }
}
