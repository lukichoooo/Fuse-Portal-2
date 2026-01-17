using MediatR;

namespace FusePortal.Domain.Entities.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageRemovedEvent(Guid ChatId, Guid MessageId) : INotification;
}
