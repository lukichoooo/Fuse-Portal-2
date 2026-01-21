using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveLecturerFromSubjectCommand
{
    public class RemoveLecturerFromSubjectCommandHandler : BaseCommandHandler<RemoveLecturerFromSubjectCommand>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public RemoveLecturerFromSubjectCommandHandler(
                IIdentityProvider identity,
                ISubjectRepo repo,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
        }

        protected override async Task ExecuteAsync(RemoveLecturerFromSubjectCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = await _repo.GetByIdAsync(command.SubjectId, userId)
                ?? throw new SubjectNotFoundException($"Subject with Id={command.SubjectId} not found");
            subject.RemoveLecturer(command.LecturerId);
        }
    }
}
