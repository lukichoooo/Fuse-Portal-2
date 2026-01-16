using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UniversityAggregate;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents;

public sealed record UserRemovedUniversityEvent(
        int UserId,
        University University
        ) : INotification;
