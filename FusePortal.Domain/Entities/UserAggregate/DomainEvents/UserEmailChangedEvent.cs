using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.DomainEvents;

public sealed record UserEmailChangedEvent(
        Guid Id,
        string OldEmail,
        string NewEmail) : INotification;
