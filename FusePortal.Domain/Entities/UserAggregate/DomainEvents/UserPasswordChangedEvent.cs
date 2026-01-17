using MediatR;

namespace FusePortal.Domain.Entities.UserAggregate.DomainEvents
{
    public sealed record UserPasswordChangedEvent(Guid UserId) : INotification;
}
