using FusePortal.Infrastructure.Services.LLM.Interfaces;

namespace FusePortal.Infrastructure.Services.LLM.Implementation
{

    public sealed class FilePromptProvider : IPromptProvider
    {
        private readonly string _chatPrompt;
        private readonly string _parserPrompt;
        private readonly string _examGeneratorPrompt;
        private readonly string _examResultAnalyzerPrompt;

        public FilePromptProvider()
        {
            var basePath = Path.Combine(
                    "..",
                    "FusePortal.Infrastructure",
                    "Settings",
                    "LLM",
                    "Prompts");

            _chatPrompt = File.ReadAllText(Path.Combine(basePath, "chat.prompt.txt"));
            _parserPrompt = File.ReadAllText(Path.Combine(basePath, "parser.prompt.txt"));
            _examGeneratorPrompt = File.ReadAllText(Path.Combine(basePath, "exam_generator.prompt.txt"));
            _examResultAnalyzerPrompt = File.ReadAllText(Path.Combine(basePath, "exam_result_analyzer.prompt.txt"));
        }

        public string GetChatPrompt() => _chatPrompt;
        public string GetParserPrompt() => _parserPrompt;
        public string GetExamGeneratorPrompt() => _examGeneratorPrompt;
        public string GetExamResultAnalyzerPrompt() => _examResultAnalyzerPrompt;
    }

}
