using MediatR;

namespace FusePortal.Application.UseCases.Academic.Exams.Queries.GetExamById
{
    public sealed record GetExamByIdQuery(Guid ExamId) : IRequest<ExamDto>;
}
