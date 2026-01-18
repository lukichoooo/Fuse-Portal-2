using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageFileAttachedEvent(
            Guid ChatId,
            Guid MessageId,
            Guid FileId) : IDomainEvent;
}
