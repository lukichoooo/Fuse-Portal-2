using MediatR;

namespace FusePortal.Domain.Entities.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageFileAttachedEvent(
            Guid ChatId,
            Guid MessageId,
            Guid FileId) : INotification;
}
