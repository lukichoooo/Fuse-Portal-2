using FusePortal.Domain.Common.ValueObjects.Address;
using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.DomainEvents;

public sealed record UserAddressChangedEvent(
        Guid UserId,
        Address OldAddress,
        Address NewAddress) : INotification;
