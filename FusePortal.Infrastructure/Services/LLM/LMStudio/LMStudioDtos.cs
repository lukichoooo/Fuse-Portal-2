namespace FusePortal.Infrastructure.Services.LLM.LMStudio
{
    public record Content(
        string Type,
        string Text
    );

    public record Output(
        string Id,
        string Type,
        string Role,
        string Status,
        List<Content> Content
    );

    public record OutputTokensDetails(
        int ReasoningTokens
    );

    public record Usage(
        int InputTokens,
        int OutputTokens,
        int TotalTokens,
        OutputTokensDetails OutputTokensDetails
    );

    public record LMStudioResponse(
        string Id,
        string Object,
        int CreatedAt,
        string Status,
        string Model,
        List<Output> Output,
        Usage Usage,
        string PreviousResponseId
    );


    public record ResponsesAPIRequestStructured(string? JsonSchema);
    public record ResponsesAPIRequestText(ResponsesAPIRequestStructured? Structured);

    public record LMStudioRequest
    {
        public string Model { get; set; }
        public string Input { get; set; }
        public bool Stream { get; set; }
        public ResponsesAPIRequestText? Text { get; set; }
        public string? PreviousResponseId { get; set; }
    }

    public record LMStudioStreamEvent(
         string Type,
         string? ItemId = null,
         int? OutputIndex = null,
         int? ContentIndex = null,
         string? Delta = null,
         int? SequenceNumber = null,
         LMStudioResponse? Response = null
     );


    // completitions API


    public class LMStudioCompletionRequest
    {
        public string Model { get; set; } = "";
        public string Prompt { get; set; } = "";
        public ResponseFormat? ResponseFormat { get; set; }
        public double? Temperature { get; set; }
        public int? MaxTokens { get; set; }
        public bool? Stream { get; set; }
    }


    public class Message
    {
        public string Role { get; set; } = "";
        public string Content { get; set; } = "";
    }

    public class ResponseFormat
    {
        public string Type { get; set; } = "json_schema";
        public JsonSchema JsonSchema { get; set; } = new();
    }

    public class JsonSchema
    {
        public string Name { get; set; } = "";
        public string Strict { get; set; } = "true";
        public object Schema { get; set; } = new { };
    }


    public class LMStudioCompletionResponse
    {
        public string Id { get; set; } = "";
        public string Object { get; set; } = "";
        public long Created { get; set; }
        public string Model { get; set; } = "";
        public List<Choice> Choices { get; set; }
        public Usage Usage { get; set; }
    }

    public class Choice
    {
        public int Index { get; set; }
        public string Text { get; set; } = "";
        public object? LogProbs { get; set; }
        public string? FinishReason { get; set; }
    }


}
