using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;

public sealed record UserEmailChangedEvent(
        Guid Id,
        string OldEmail,
        string NewEmail) : INotification;
