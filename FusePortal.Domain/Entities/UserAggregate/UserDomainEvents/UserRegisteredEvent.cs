using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;

public sealed record UserRegisteredEvent(
        Guid Id,
        string Name,
        string Email) : INotification;
