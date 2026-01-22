using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using FusePortal.Application.Interfaces.Services.PortalTransfer;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Exceptions;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;

namespace FusePortal.Infrastructure.Services.LLM.LMStudio.Adapters.Portal
{
    public class LMStudioPortalTransferService(
            ILMStudioApi api,
            ILMStudioMapper mapper,
            ILLMApiSettingsChooser apiSettings
            ) : IPortalTransferService
    {
        private readonly ILMStudioApi _api = api;
        private readonly ILMStudioMapper _mapper = mapper;
        private readonly ILLMApiSettingsChooser _apiSettings = apiSettings;
        private record PortalLLMDto(List<SubjectLLMDto> Subjects);

        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        public async Task<List<SubjectLLMDto>> SavePortalAsync(string page)
        {
            page = CleanHtml(page);

            LMStudioRequest lmStudioRequest = _mapper.ToRequest(
                   text: page,
                    rulesPrompt: _apiSettings.GetParserPrompt());

            LMStudioResponse response = await _api.SendMessageAsync(
                    lmStudioRequest,
                    _apiSettings.GetParserSettings()
                    );

            var portalText = _mapper.ToOutputText(response);

            var portalJson = ExtractJsonObject(portalText);


            return JsonSerializer.Deserialize<PortalLLMDto>
                (portalJson, _serializerOptions)?.Subjects
                   ?? throw new LMStudioApiException("PortalTransferService: LMStudio returned empty response");
        }



        // helper

        public string ExtractJsonObject(string text)
        {
            int depth = 0;
            int start = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '{')
                {
                    if (depth == 0)
                        start = i;
                    depth++;
                }
                else if (text[i] == '}')
                {
                    depth--;
                    if (depth == 0 && start != -1)
                        return text.Substring(start, i - start + 1);
                }
                if (depth < 0)
                    break;
            }

            throw new LMStudioJsonParseException("No valid JSON object found.");
        }


        private string CleanHtml(string html)
        {
            html = Regex.Replace(html, "<script[^>]*?>[\\s\\S]*?</script>", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, "<style[^>]*?>[\\s\\S]*?</style>", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, "<!--.*?-->", "", RegexOptions.Singleline);

            html = Regex.Replace(html, "<img[^>]+>", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, @"\s+", " ", RegexOptions.Multiline);

            return html.Trim();
        }

    }
}
