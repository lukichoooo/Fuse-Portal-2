using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate.SubjectDomainEvents
{
    public record SubjectSyllabusAddedEvent(
            Guid SubjectId,
            Syllabus Syllabus) : IDomainEvent;
}
