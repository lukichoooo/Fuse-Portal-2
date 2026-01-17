using Facet.Extensions;
using FusePortal.Application.Unis.Exceptions;
using FusePortal.Domain.Entities.UniversityAggregate;
using MediatR;

namespace FusePortal.Application.Unis.Queries.GetUniById
{
    public class GetUniByIdQueryHandler(IUniRepo repo)
        : IRequestHandler<GetUniByIdQuery, UniDto>
    {
        private readonly IUniRepo _repo = repo;

        public async Task<UniDto> Handle(GetUniByIdQuery request, CancellationToken cancellationToken)
        {
            var uni = await _repo.GetByIdAsync(request.Id)
                ?? throw new UniNotFoundException($"Uni Not Found With Id={request.Id}");

            return uni.ToFacet<University, UniDto>();
        }
    }
}
