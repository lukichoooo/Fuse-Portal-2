using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate.UserDomainEvents
{
    public sealed record UserRegisteredEvent(
            int UserId,
            string Name,
            Address Address) : INotification;
}
