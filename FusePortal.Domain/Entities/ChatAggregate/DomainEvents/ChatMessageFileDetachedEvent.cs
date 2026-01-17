using MediatR;

namespace FusePortal.Domain.Entities.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageFileDetachedEvent(
            Guid ChatId,
            Guid MessageId,
            Guid FileId) : INotification;
}
