using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserRemovedUniversityEvent(
        Guid UserId,
        Guid UniId
        ) : IDomainEvent;
