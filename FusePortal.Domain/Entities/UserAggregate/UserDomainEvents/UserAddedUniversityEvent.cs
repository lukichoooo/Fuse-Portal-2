using FusePortal.Domain.Entities.UniversityAggregate;
using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.UserDomainEvents;

public sealed record UserAddedUniversityEvent(
        Guid UserId,
        University University
        ) : INotification;
