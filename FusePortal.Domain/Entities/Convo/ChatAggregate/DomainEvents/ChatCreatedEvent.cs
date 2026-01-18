using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate.DomainEvents
{
    public sealed record ChatCreatedEvent(Guid ChatId, Guid UserId) : IDomainEvent;
}
