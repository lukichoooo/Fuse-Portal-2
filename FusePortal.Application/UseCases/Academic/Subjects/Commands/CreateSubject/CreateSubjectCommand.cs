using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.CreateSubject
{
    public sealed record CreateSubjectCommand(
            string Name,
            string? Metadata = null) : IRequest<Guid>;
}
