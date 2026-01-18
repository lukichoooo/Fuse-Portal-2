using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserEmailChangedEvent(
        Guid Id,
        string OldEmail,
        string NewEmail) : IDomainEvent;
