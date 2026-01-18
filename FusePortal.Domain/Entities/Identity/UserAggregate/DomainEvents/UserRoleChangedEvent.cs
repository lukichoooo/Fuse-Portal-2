using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserRoleChangedEvent(
        Guid Id,
        RoleType OldRole,
        RoleType NewRole) : IDomainEvent;
