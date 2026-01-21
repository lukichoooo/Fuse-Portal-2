using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusForSubject
{
    public sealed record CreateSyllabusForSubjectCommand(
                string Name,
                string Content,
                Guid SubjectId) : IRequest;
}
