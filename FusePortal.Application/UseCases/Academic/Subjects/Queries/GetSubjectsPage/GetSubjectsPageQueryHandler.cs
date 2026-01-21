using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Queries.GetSubjectsPage
{
    public class GetSubjectsPageQueryHandler : IRequestHandler<GetSubjectsPageQuery, List<SubjectDto>>
    {
        private readonly ISubjectRepo _repo;
        private readonly IIdentityProvider _identity;

        public GetSubjectsPageQueryHandler(
            ISubjectRepo repo,
            IIdentityProvider identity
                )
        {
            _repo = repo;
            _identity = identity;
        }


        public async Task<List<SubjectDto>> Handle(GetSubjectsPageQuery request, CancellationToken cancellationToken)
        {
            var userId = _identity.GetCurrentUserId();
            var subjects = await _repo.GetPageAsync(request.LastId, request.PageSize, userId);
            return subjects.SelectFacets<Subject, SubjectDto>()
                .ToList();
        }
    }
}
