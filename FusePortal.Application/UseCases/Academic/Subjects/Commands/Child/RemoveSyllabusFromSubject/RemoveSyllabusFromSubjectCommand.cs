using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveSyllabusFromSubject
{
    public sealed record RemoveSyllabusFromSubjectCommand(
            Guid SubjectId,
            Guid SyllabusId) : IRequest;
}
