using Facet.Extensions;
using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.UseCases.Academic.Exams.Exceptions;
using FusePortal.Application.UseCases.Academic.Exams.Services;
using FusePortal.Domain.Entities.Academic.ExamAggregate;

namespace FusePortal.Application.UseCases.Academic.Exams.Commands.GradeExam
{
    public class GradeExamCommandHandler : BaseCommandHandler<GradeExamCommand, Guid>
    {

        private readonly IExamRepo _repo;
        private readonly IExamService _examService;

        public GradeExamCommandHandler(
                IExamRepo repo,
                IExamService examService,
                IUnitOfWork uow
                ) : base(uow)
        {
            _repo = repo;
            _examService = examService;
        }

        protected override async Task<Guid> ExecuteAsync(GradeExamCommand command, CancellationToken ct)
        {
            var exam = await _repo.FindById(command.ExamId)
                ?? throw new ExamNotFoundException($"exam with Id={command.ExamId}");

            (int? scoreFrom100, string results) =
                await _examService.GradeExamAsync(exam.ToFacet<Exam, ExamDto>(), ct);

            exam.UpdateExamGrade(results, scoreFrom100);
            return exam.Id;
        }
    }
}
