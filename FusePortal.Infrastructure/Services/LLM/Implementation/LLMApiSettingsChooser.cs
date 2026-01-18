using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Settings.LLM;
using Microsoft.Extensions.Options;

namespace FusePortal.Infrastructure.Services.LLM.Implementation
{
    public class LLMApiSettingsChooser : ILLMApiSettingsChooser
    {
        private readonly LLMApiSettingKeys _apiKeys;

        private readonly LLMApiSettings _chatSettings;
        private readonly LLMApiSettings _parserSettings;
        private readonly LLMApiSettings _examSettings;

        private readonly IPromptProvider _promptProvider;

        public LLMApiSettingsChooser(
            IOptions<LLMApiSettingKeys> keyOptions,
            IOptionsMonitor<LLMApiSettings> apiOptionsMonitor,
            IPromptProvider promptProvider)
        {
            _apiKeys = keyOptions.Value;

            _chatSettings = apiOptionsMonitor.Get(_apiKeys.Chat);
            _parserSettings = apiOptionsMonitor.Get(_apiKeys.Parser);
            _examSettings = apiOptionsMonitor.Get(_apiKeys.Exam);

            _promptProvider = promptProvider;
        }

        // chat
        public string GetChatPrompt()
            => _promptProvider.GetChatPrompt();

        public LLMApiSettings GetChatSettings()
            => _chatSettings;



        // exam
        public string GetExamGeneratorPrompt()
            => _promptProvider.GetExamGeneratorPrompt();

        public string GetExamResultAnalyzerPrompt()
            => _promptProvider.GetExamResultAnalyzerPrompt();

        public LLMApiSettings GetExamGeneratorSettings()
            => _examSettings;



        // parser
        public string GetParserPrompt()
            => _promptProvider.GetParserPrompt();

        public LLMApiSettings GetParserSettings()
            => _parserSettings;
    }
}
