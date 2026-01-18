using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents
{
    public sealed record UserPasswordChangedEvent(Guid UserId) : IDomainEvent;
}
