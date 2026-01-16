using MediatR;

namespace FusePortal.Domain.Entities.SubjectAggregate.SubjectDomainEvents
{
    public record SubjectSyllabusAddedEvent(
            Guid SubjectId,
            Syllabus Syllabus) : INotification;
}
