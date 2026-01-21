using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveLecturerFromSubjectCommand
{
    public sealed record RemoveLecturerFromSubjectCommand(
            Guid SubjectId,
            Guid LecturerId) : IRequest;
}
