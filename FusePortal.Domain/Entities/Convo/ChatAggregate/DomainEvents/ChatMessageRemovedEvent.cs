using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents
{
    public sealed record ChatMessageRemovedEvent(Guid ChatId, Guid MessageId)
        : IDomainEvent;
}
