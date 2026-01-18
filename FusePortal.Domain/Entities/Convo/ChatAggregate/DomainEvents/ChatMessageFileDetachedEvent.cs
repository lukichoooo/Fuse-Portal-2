using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageFileDetachedEvent(
            Guid ChatId,
            Guid MessageId,
            Guid FileId) : IDomainEvent;
}
