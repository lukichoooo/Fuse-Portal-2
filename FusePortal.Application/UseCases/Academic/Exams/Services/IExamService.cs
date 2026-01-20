using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Exams.Services
{
    public interface IExamService
    {
        Task<string> GenerateExamQuestionsAsync(Subject subject, CancellationToken ct = default);

        Task<(int? scoreFrom100, string results)> GradeExamAsync(ExamDto examDto, CancellationToken ct = default);
    }
}
