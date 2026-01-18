namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface IPromptProvider
    {
        string GetChatPrompt();
        string GetParserPrompt();
        string GetExamGeneratorPrompt();
        string GetExamResultAnalyzerPrompt();
    }
}
