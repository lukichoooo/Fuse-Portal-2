using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.AddLecturerToSubjectCommand
{
    public class AddLecturerToSubjectCommandHandler : BaseCommandHandler<AddLecturerToSubjectCommand>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public AddLecturerToSubjectCommandHandler(
                ISubjectRepo repo,
                IIdentityProvider identity,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
        }

        protected override Task ExecuteAsync(AddLecturerToSubjectCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = _repo.FindById(command.SubjectId, userId)
                ?? throw new SubjectNotFoundException($"Subject with Id={command.SubjectId} not found");
            // subject.AddLecturer(); TODO:
            return Task.CompletedTask;
        }
    }
}
