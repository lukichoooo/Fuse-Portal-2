using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.RemoveSubject
{
    public sealed record RemoveSubjectCommand(Guid SubjectId) : IRequest;
}
