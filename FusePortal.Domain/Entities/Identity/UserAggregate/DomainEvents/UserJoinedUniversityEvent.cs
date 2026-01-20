
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserJoinedUniversityEvent(
        Guid UserId,
        Guid UniId
        ) : IDomainEvent;
