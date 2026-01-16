using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents;

public sealed record UserRegisteredEvent(
        string Name,
        string Email,
        Address Address) : INotification;
