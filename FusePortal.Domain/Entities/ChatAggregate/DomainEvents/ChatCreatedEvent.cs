using MediatR;

namespace FusePortal.Domain.Entities.ChatAggregate.DomainEvents
{
    public sealed record ChatCreatedEvent(Guid ChatId, Guid UserId) : INotification;
}
