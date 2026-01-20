using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Exams.Services;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.ExamAggregate;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Exams.Commands.GenerateExam
{
    public class GenerateExamCommandHandler : BaseCommandHandler<GenerateExamCommand, Guid>
    {
        private readonly IExamRepo _repo;
        private readonly ISubjectRepo _subjectRepo;
        private readonly IExamService _examService;
        private readonly IIdentityProvider _identity;

        public GenerateExamCommandHandler(
                IExamRepo repo,
                ISubjectRepo subjectRepo,
                IExamService examService,
                IIdentityProvider identity,
                IUnitOfWork uow
                ) : base(uow)
        {
            _repo = repo;
            _subjectRepo = subjectRepo;
            _identity = identity;
            _examService = examService;
        }

        protected override async Task<Guid> ExecuteAsync(GenerateExamCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = await _subjectRepo.FindById(command.SubjectId, userId)
                ?? throw new SubjectNotFoundException(@$"subject with SyllabusId={command.SubjectId}
                        not found For UserId={userId}");

            var questions = await _examService.GenerateExamQuestionsAsync(subject, ct);
            var exam = new Exam(questions, subject.Id);
            await _repo.AddAsync(exam);
            return exam.Id;
        }
    }
}
