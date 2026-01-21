using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Queries.GetSubjectById
{
    public sealed record GetSubjectByIdQuery(Guid SubjectId) : IRequest<SubjectDto>;
}
