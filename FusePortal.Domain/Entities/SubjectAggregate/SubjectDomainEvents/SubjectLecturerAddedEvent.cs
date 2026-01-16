using MediatR;

namespace FusePortal.Domain.Entities.SubjectAggregate.SubjectDomainEvents
{
    public sealed record SubjectLecturerAddedEvent(
            Guid SubjectId,
            Lecturer Lecturer) : INotification;
}
