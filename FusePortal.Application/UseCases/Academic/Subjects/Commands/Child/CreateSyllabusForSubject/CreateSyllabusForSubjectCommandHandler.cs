using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusForSubject
{
    public class CreateSyllabusForSubjectCommandHandler : BaseCommandHandler<CreateSyllabusForSubjectCommand>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public CreateSyllabusForSubjectCommandHandler(
                IIdentityProvider identity,
                ISubjectRepo repo,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
        }

        protected override async Task ExecuteAsync(CreateSyllabusForSubjectCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = await _repo.GetByIdAsync(command.SubjectId, userId)
                ?? throw new SubjectNotFoundException($"Subject with Id={command.SubjectId} not found");
            subject.AddSyllabus(command.Name, command.Content);
        }
    }
}
