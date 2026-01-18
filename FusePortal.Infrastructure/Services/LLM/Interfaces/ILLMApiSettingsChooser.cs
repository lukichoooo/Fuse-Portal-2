using FusePortal.Infrastructure.Settings.LLM;

namespace FusePortal.Infrastructure.Services.LLM.Interfaces
{
    public interface ILLMApiSettingsChooser
    {
        LLMApiSettings GetChatSettings();
        LLMApiSettings GetParserSettings();
        LLMApiSettings GetExamGeneratorSettings();

        string GetChatPrompt();
        string GetParserPrompt();
        string GetExamGeneratorPrompt();
    }
}
