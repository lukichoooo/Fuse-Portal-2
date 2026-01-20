using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate.ExamDomainEvents
{
    public sealed record ExamResultUpdatedEvent(
            Guid ExamId,
            int? OldGrade,
            int? NewGrade
            ) : IDomainEvent;
}
