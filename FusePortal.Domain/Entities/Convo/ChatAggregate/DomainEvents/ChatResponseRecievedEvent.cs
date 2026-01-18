using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents
{
    public sealed record ChatResponseRecievedEvent(Guid ChatId, Guid MessageId)
        : IDomainEvent;
}
