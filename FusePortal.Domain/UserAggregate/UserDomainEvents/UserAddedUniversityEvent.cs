using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UniversityAggregate;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents;

public sealed record UserAddedUniversityEvent(
        int UserId,
        University University
        ) : INotification;
