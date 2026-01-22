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

        private readonly IFileReader _fileProvider;

        public LLMApiSettingsChooser(
            IOptions<LLMApiSettingKeys> keyOptions,
            IOptionsMonitor<LLMApiSettings> apiOptionsMonitor,
            IFileReader promptProvider)
        {
            _apiKeys = keyOptions.Value;

            _chatSettings = apiOptionsMonitor.Get(_apiKeys.Chat);
            _parserSettings = apiOptionsMonitor.Get(_apiKeys.Parser);
            _examSettings = apiOptionsMonitor.Get(_apiKeys.Exam);

            _fileProvider = promptProvider;
        }

        // chat
        public string GetChatPrompt()
            => _fileProvider.GetChatPrompt();

        public LLMApiSettings GetChatSettings()
            => _chatSettings;



        // exam
        public string GetExamGeneratorPrompt()
            => _fileProvider.GetExamGeneratorPrompt();

        public string GetExamResultGraderPrompt()
            => _fileProvider.GetExamResultAnalyzerPrompt();

        public LLMApiSettings GetExamServiceSettings()
            => _examSettings;




        // parser
        public string GetParserPrompt()
            => _fileProvider.GetParserPrompt();

        public string GetParserSchema()
            => _fileProvider.GetParserSchema();

        public LLMApiSettings GetParserSettings()
            => _parserSettings;
    }
}
