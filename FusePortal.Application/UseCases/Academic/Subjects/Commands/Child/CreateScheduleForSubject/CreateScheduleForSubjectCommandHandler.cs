using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateScheduleForSubject
{
    public class CreateScheduleForSubjectCommandHandler : BaseCommandHandler<CreateScheduleForSubjectCommand>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public CreateScheduleForSubjectCommandHandler(
                ISubjectRepo repo,
                IIdentityProvider identity,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
        }

        protected override async Task ExecuteAsync(CreateScheduleForSubjectCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var subject = await _repo.GetByIdAsync(command.SubjectId, userId)
                ?? throw new SubjectNotFoundException($"Subject with Id={command.SubjectId} not found");
            subject.AddSchedule(command.LectureDate, command.Location, command.Metadata);
        }
    }
}
