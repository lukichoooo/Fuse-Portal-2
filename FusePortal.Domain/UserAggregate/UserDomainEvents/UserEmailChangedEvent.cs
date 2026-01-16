using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents;

public sealed record UserEmailChangedEvent(
        int Id,
        string OldEmail,
        string NewEmail) : INotification;
