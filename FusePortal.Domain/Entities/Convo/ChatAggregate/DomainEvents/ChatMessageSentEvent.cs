using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageSentEvent(Guid ChatId, Guid MessageId)
        : IDomainEvent;
}
