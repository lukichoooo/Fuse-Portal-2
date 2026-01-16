using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;

public sealed record UserRoleChangedEvent(
        Guid Id,
        RoleType OldRole,
        RoleType NewRole) : INotification;
