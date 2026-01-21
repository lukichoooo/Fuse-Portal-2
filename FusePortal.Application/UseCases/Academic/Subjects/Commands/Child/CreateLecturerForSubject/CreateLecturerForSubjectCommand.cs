using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateLecturerForSubject
{
    public sealed record CreateLecturerForSubjectCommand(Guid SubjectId, string Name) : IRequest;
}
