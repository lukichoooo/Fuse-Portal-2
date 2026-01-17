using MediatR;

namespace FusePortal.Domain.Entities.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageSentEvent(Guid ChatId, Guid MessageId) : INotification;
}
