using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveScheduleFromSubjectCommand
{
    public sealed record RemoveScheduleFromSubjectCommand(
            Guid SubjectId,
            Guid ScheduleId) : IRequest;
}
