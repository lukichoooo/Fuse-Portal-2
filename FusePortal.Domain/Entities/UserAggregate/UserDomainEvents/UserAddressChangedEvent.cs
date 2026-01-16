using FusePortal.Domain.Common.ValueObjects;
using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;

public sealed record UserAddressChangedEvent(
        Guid UserId,
        Address OldAddress,
        Address NewAddress) : INotification;
