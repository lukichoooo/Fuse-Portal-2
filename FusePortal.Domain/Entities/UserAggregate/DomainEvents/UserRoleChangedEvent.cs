using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.DomainEvents;

public sealed record UserRoleChangedEvent(
        Guid Id,
        RoleType OldRole,
        RoleType NewRole) : INotification;
