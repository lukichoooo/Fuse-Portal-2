using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate.SubjectDomainEvents
{
    public sealed record SubjectCreatedEvent(
            string Name,
            Guid UserId) : IDomainEvent;
}
