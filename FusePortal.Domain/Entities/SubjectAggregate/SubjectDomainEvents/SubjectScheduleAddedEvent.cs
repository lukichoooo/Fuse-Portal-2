using MediatR;

namespace FusePortal.Domain.Entities.SubjectAggregate.SubjectDomainEvents
{
    public sealed record SubjectScheduleAddedEvent(
            Guid SubjectId,
            Schedule Schedule) : INotification;
}
