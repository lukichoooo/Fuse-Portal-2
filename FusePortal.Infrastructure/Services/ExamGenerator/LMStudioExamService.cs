using System.Text;
using FusePortal.Application.UseCases.Academic.Exams;
using FusePortal.Application.UseCases.Academic.Exams.Services;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Infrastructure.Services.ExamGenerator.Exceptions;
using FusePortal.Infrastructure.Services.LLM.Interfaces;
using FusePortal.Infrastructure.Services.LLM.LMStudio.Interfaces;

namespace FusePortal.Infrastructure.Services.ExamGenerator
{
    public class LMStudioExamService(
            ILMStudioApi api,
            ILMStudioMapper mapper,
            ILLMApiSettingsChooser apiSettings
            ) : IExamService
    {
        private readonly ILMStudioApi _api = api;
        private readonly ILMStudioMapper _mapper = mapper;
        private readonly ILLMApiSettingsChooser _apiSettings = apiSettings;


        public async Task<string> GenerateExamQuestionsAsync(Subject subject, CancellationToken ct = default)
        {
            StringBuilder sb = new();
            foreach (var syllabus in subject.Syllabuses)
                sb.AppendLine(syllabus.Content);

            var syllabusData = sb.ToString();
            var request = _mapper.ToRequest(
                    syllabusData,
                    _apiSettings.GetExamGeneratorPrompt());

            var result = await _api.SendMessageAsync(
                    request,
                    _apiSettings.GetExamServiceSettings(),
                    ct);

            return _mapper.ToOutputText(result);
        }


        public async Task<(int? scoreFrom100, string results)> GradeExamAsync(ExamDto examDto, CancellationToken ct = default)
        {
            var request = _mapper.ToRequest(
                    examDto,
                    _apiSettings.GetExamResultGraderPrompt());

            var response = await _api.SendMessageAsync(
                    request,
                    _apiSettings.GetExamServiceSettings(),
                    ct);

            var result = _mapper.ToOutputText(response);

            int? scoreFrom100 = ExtractGradeFromResponseWithScore(result);
            scoreFrom100 ??= ExtractGradeFromResponseWithPercentage(result);

            return (scoreFrom100, result);
        }


        // Helper

        private static int ExtractGradeFromResponseWithPercentage(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                throw new ArgumentException("Response cannot be null or empty", nameof(response));

            int percentIndex = response.LastIndexOf('%');
            if (percentIndex == -1) throw new ExamScoreParsingException("Score not found");

            int grade = 0, multiplier = 1;
            for (int i = percentIndex - 1; i >= 0; i--)
            {
                if (char.IsDigit(response[i]))
                {
                    grade += (response[i] - '0') * multiplier;
                    multiplier *= 10;
                }
                else if (grade > 0)
                {
                    break; // stop if digits ended
                }
            }

            return grade;
        }

        private static int? ExtractGradeFromResponseWithScore(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                throw new ArgumentException("Response cannot be null or empty", nameof(response));

            var scoreIndex = response.LastIndexOf("Score:", StringComparison.OrdinalIgnoreCase);

            if (scoreIndex == -1)
                throw new ExamScoreParsingException("Score not found in response");

            var scoreText = response[(scoreIndex + 6)..].Trim();

            scoreText = new string(scoreText.TakeWhile(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(scoreText) || !int.TryParse(scoreText, out int grade))
                return null;

            return grade;
        }

    }
}
