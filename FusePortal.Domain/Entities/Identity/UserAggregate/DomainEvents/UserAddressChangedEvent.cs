using FusePortal.Domain.Common.ValueObjects.Address;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Identity.UserAggregate.DomainEvents;

public sealed record UserAddressChangedEvent(
        Guid UserId,
        Address OldAddress,
        Address NewAddress) : IDomainEvent;
