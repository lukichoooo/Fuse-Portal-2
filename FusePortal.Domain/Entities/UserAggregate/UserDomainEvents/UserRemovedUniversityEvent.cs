using FusePortal.Domain.Entities.UniversityAggregate;
using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;

public sealed record UserRemovedUniversityEvent(
        Guid UserId,
        University University
        ) : INotification;
