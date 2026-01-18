using FusePortal.Domain.Common.ValueObjects.Address;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.UniversityAggregate.UniversityDomainEvents;

public sealed record UniversityCreatedEvent(
        Guid Id,
        string Name,
        Address Address) : IDomainEvent;
