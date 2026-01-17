using FusePortal.Domain.Common.ValueObjects.Address;
using MediatR;

namespace FusePortal.Domain.Entities.UniversityAggregate.UniversityDomainEvents;

public sealed record UniversityCreatedEvent(
        Guid Id,
        string Name,
        Address Address) : INotification;
