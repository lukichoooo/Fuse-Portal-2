using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Queries.GetSubjectsPage
{
    public sealed record GetSubjectsPageQuery(Guid? LastId, int PageSize)
        : IRequest<List<SubjectDto>>;
}
