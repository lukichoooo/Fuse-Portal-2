using Facet.Extensions;
using FusePortal.Domain.Entities.UniversityAggregate;
using MediatR;

namespace FusePortal.Application.Unis.Queries.GetUnisPage
{
    public class GetUnisPageQueryHandler(IUniRepo repo)
        : IRequestHandler<GetUnisPageQuery, List<UniDto>>
    {
        private readonly IUniRepo _repo = repo;
        public async Task<List<UniDto>> Handle(GetUnisPageQuery request, CancellationToken cancellationToken)
        {
            var unis = await _repo.GetPageAsync(request.LastId, request.PageSize);
            return unis
                .SelectFacets<University, UniDto>()
                .ToList();
        }
    }
}
