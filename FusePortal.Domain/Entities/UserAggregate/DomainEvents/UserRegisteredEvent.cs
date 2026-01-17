using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.DomainEvents;

public sealed record UserRegisteredEvent(
        Guid Id,
        string Name,
        string Email) : INotification;
