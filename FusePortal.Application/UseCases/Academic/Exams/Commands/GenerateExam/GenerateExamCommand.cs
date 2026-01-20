using MediatR;

namespace FusePortal.Application.UseCases.Academic.Exams.Commands.GenerateExam
{
    public sealed record GenerateExamCommand(Guid SubjectId) : IRequest<Guid>;
}
