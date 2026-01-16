using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UniversityAggregate.UniversityDomainEvents;

public sealed record UniversityCreatedEvent(
        string Name,
        Address Address) : INotification;
