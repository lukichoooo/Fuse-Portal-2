using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.CreateSubject
{
    public class CreateSubjectCommandHandler : BaseCommandHandler<CreateSubjectCommand, Guid>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public CreateSubjectCommandHandler(
                ISubjectRepo repo,
                IIdentityProvider identity,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
        }

        protected override async Task<Guid> ExecuteAsync(CreateSubjectCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = new Subject(command.Name, userId, command.Metadata);
            await _repo.AddAsync(subject);
            return subject.Id;
        }
    }
}
