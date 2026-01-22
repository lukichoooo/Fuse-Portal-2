using FusePortal.Infrastructure.Settings.LLM;

namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface ILLMApiSettingsChooser
    {
        LLMApiSettings GetChatSettings();

        LLMApiSettings GetParserSettings();

        LLMApiSettings GetExamServiceSettings();

        string GetChatPrompt();

        string GetParserPrompt();
        string GetParserSchema();

        string GetExamGeneratorPrompt();
        string GetExamResultGraderPrompt();
    }
}
