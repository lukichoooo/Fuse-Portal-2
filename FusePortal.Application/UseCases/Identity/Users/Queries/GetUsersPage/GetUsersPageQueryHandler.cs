using Facet.Extensions;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Queries.GetUsersPage
{
    public class GetUsersPageQueryHandler(IUserRepo repo) :
        IRequestHandler<GetUsersPageQuery, List<UserDto>>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<List<UserDto>> Handle(GetUsersPageQuery request, CancellationToken cancellationToken)
        {
            var users = await _repo.GetPageAsync(request.LastId, request.PageSize);
            return users
                .SelectFacets<User, UserDto>()
                .ToList();
        }
    }
}
