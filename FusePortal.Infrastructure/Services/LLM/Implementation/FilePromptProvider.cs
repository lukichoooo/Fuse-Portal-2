using FusePortal.Infrastructure.Services.LLM.Interfaces;
using Microsoft.Extensions.Hosting;

namespace FusePortal.Infrastructure.Services.LLM.Implementation
{
    public sealed class FilePromptProvider(IHostEnvironment env) : IPromptProvider
    {
        // TODO: needs checking if works
        private readonly string _basePath = Path.Combine(
                env.ContentRootPath,
                "Settings",
                "LLM",
                "Prompts"
            );

        public string GetChatPrompt()
            => File.ReadAllText(Path.Combine(_basePath, "chat.prompt.txt"));

        public string GetParserPrompt()
            => File.ReadAllText(Path.Combine(_basePath, "parser.prompt.txt"));

        public string GetExamGeneratorPrompt()
            => File.ReadAllText(Path.Combine(_basePath, "exam_generator.prompt.txt"));

        public string GetExamResultAnalyzerPrompt()
            => File.ReadAllText(Path.Combine(_basePath, "exam_result_analyzer.prompt.txt"));
    }
}
