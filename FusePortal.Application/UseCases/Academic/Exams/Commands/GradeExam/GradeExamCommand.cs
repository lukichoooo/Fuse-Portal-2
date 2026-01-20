using MediatR;

namespace FusePortal.Application.UseCases.Academic.Exams.Commands.GradeExam
{
    public sealed record GradeExamCommand(Guid ExamId, string Answers) : IRequest<Guid>;
}
