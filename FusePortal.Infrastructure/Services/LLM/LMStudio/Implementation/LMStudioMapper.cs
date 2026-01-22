using FusePortal.Application.UseCases.Academic.Exams;
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
           => new(
                Text: response.Output[0].Content[0].Text,
                ChatId: chatId,
                Files: []
            );

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


        public LMStudioCompletionRequest ToCompletionRequest(
            string text,
            string? rulesPrompt = null,
            string? jsonSchema = null)
        {
            var request = new LMStudioCompletionRequest
            {
                Model = _settingsChooser.GetParserSettings().Model,
                Prompt = _inputGenerator.GenerateInput(text, rulesPrompt)
            };

            if (!string.IsNullOrWhiteSpace(jsonSchema))
            {
                request.ResponseFormat = new ResponseFormat
                {
                    Type = "json_schema",
                    JsonSchema = new JsonSchema
                    {
                        Schema = System.Text.Json.JsonSerializer.Deserialize<object>(jsonSchema) ?? new { }
                    }
                };
            }

            return request;
        }



        public string ToOutputText(LMStudioResponse response)
            => response.Output[0].Content[0].Text;

        public LMStudioRequest ToRequest(ExamDto examDto, string? rulesPrompt = null)
            => new()
            {
                Model = _settingsChooser.GetExamServiceSettings().Model,
                Input = _inputGenerator.GenerateInput(examDto, rulesPrompt),
            };
    }
}
