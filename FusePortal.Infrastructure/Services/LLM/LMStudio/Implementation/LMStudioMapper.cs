using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Implementation
{
    public class LMStudioMapper : ILMStudioMapper
    {
        private readonly ILLMApiSettingsChooser _settingsChooser;
        private readonly ILLMInputGenerator _inputGenerator;

        public LMStudioMapper(
                ILLMInputGenerator inputGenerator,
                ILLMApiSettingsChooser apiSettingsChooser)
        {
            _inputGenerator = inputGenerator;
            _settingsChooser = apiSettingsChooser;
        }

        public MessageLLMDto ToMessageDto(LMStudioResponse response, Guid chatId)
            => new()
            {
                Text = response.Output[0].Content[0].Text,
                CreatedAt = DateTimeOffset
                        .FromUnixTimeSeconds(response.CreatedAt)
                        .UtcDateTime,
                FromUser = false,
                ChatId = chatId,
                Files = []
            };

        public LMStudioRequest ToRequest(
                MessageLLMDto msg,
                string? previousResponseId = null,
                string? rulesPrompt = null)
            => new()
            {
                Model = _settingsChooser.GetChatSettings().Model,
                Input = _inputGenerator.GenerateInput(msg, rulesPrompt),
                PreviousResponseId = previousResponseId
            };

        public LMStudioRequest ToRequest(
                string text,
                string? rulesPrompt = null)
            => new()
            {
                Model = _settingsChooser.GetParserSettings().Model,
                Input = _inputGenerator.GenerateInput(text, rulesPrompt),
            };

        public string ToOutputText(LMStudioResponse response)
            => response.Output[0].Content[0].Text;

        // public LMStudioRequest ToRequest(ExamDto examDto, string? rulesPrompt = null)
        //     => new()
        //     {
        //         Model = _settingsChooser.GetExamGeneratorSettings().Model,
        //         Input = _inputGenerator.GenerateInput(examDto, rulesPrompt),
        //     };
    }
}
