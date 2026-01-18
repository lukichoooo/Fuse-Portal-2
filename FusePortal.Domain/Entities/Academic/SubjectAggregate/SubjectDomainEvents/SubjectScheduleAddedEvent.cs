using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate.SubjectDomainEvents
{
    public sealed record SubjectScheduleAddedEvent(
            Guid SubjectId,
            Schedule Schedule) : IDomainEvent;
}
