using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents;

public sealed record UserAddressChangedEvent(
        int UserId,
        Address OldAddress,
        Address NewAddress) : INotification;
