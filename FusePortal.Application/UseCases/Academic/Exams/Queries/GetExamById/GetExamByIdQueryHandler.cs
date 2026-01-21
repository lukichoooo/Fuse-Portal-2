using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Exams.Exceptions;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.ExamAggregate;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Academic.Exams.Queries.GetExamById
{
    public class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, ExamDto>
    {
        private readonly IExamRepo _repo;
        private readonly ISubjectRepo _subjectRepo;
        private readonly IIdentityProvider _identity;

        public GetExamByIdQueryHandler(
                IExamRepo repo,
                ISubjectRepo subjectRepo,
                IIdentityProvider identity
                )
        {
            _repo = repo;
            _identity = identity;
            _subjectRepo = subjectRepo;
        }

        public async Task<ExamDto> Handle(GetExamByIdQuery request, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var exam = await _repo.FindById(request.ExamId)
                ?? throw new ExamNotFoundException($"exam with Id={request.ExamId}");

            var subject = await _subjectRepo.GetByIdAsync(exam.SubjectId, userId);
            if (subject is null)
            {
                throw new SubjectNotFoundException(@$"subject with SyllabusId={exam.SubjectId}
                        not found For UserId={userId}, examId={request.ExamId}");
            }

            return exam.ToFacet<Exam, ExamDto>();
        }
    }
}
