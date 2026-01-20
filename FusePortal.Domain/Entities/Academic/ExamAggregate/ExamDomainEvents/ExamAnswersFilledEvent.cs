using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate.ExamDomainEvents
{
    public sealed record ExamAnswersFilledEvent(Guid ExamId, string Answers) : IDomainEvent;
}
