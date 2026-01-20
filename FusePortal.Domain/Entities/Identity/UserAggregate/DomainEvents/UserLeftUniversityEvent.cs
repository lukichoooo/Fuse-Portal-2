using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserLeftUniversityEvent(
        Guid UserId,
        Guid UniId
        ) : IDomainEvent;
