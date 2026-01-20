using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate.ExamDomainEvents
{
    public sealed record ExamGradedEvent(
            Guid ExamId,
            int? Grade
            ) : IDomainEvent;
}
