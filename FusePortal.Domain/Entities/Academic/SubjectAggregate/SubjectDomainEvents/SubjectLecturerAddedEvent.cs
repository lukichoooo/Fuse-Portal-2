using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate.SubjectDomainEvents
{
    public sealed record SubjectLecturerAddedEvent(
            Guid SubjectId,
            Lecturer Lecturer) : IDomainEvent;
}
