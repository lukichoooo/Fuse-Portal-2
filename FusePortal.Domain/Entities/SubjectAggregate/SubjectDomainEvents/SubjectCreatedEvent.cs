using MediatR;

namespace FusePortal.Domain.Entities.SubjectAggregate.SubjectDomainEvents
{
    public sealed record SubjectCreatedEvent(
            string Name,
            Guid UserId) : INotification;
}
