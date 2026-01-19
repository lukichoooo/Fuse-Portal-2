using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate.ExamDomainEvents
{
    public sealed record ExamCreatedEvent(Guid ExamId) : IDomainEvent;
}
