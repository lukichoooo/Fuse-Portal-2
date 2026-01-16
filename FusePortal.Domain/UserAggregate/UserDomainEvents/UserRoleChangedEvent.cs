using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents;

public sealed record UserRoleChangedEvent(
        int Id,
        RoleType OldRole,
        RoleType NewRole) : INotification;
