
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserAddedUniversityEvent(
        Guid UserId,
        Guid UniId
        ) : IDomainEvent;
