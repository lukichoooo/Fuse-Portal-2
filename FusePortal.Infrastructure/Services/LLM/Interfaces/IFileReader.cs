namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface IFileReader
    {
        string GetChatPrompt();

        string GetParserPrompt();
        string GetParserSchema();

        string GetExamGeneratorPrompt();
        string GetExamResultAnalyzerPrompt();
    }
}
