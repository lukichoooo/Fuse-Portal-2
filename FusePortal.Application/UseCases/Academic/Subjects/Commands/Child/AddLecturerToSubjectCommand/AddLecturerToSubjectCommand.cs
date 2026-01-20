using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.AddLecturerToSubjectCommand
{
    public sealed record AddLecturerToSubjectCommand(Guid SubjectId, Guid LecturerId) : IRequest;
}
