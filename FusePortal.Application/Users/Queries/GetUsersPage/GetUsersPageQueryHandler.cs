using Facet.Extensions;
using FusePortal.Domain.Entities.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Queries.GetUsersPage
{
    public class GetUsersPageQueryHandler(IUserRepo repo) :
        IRequestHandler<GetUsersPageQuery, List<UserDto>>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<List<UserDto>> Handle(GetUsersPageQuery request, CancellationToken cancellationToken)
        {
            var users = await _repo.GetPageAsync(request.LastId, request.PageSize); // TODO: give real args
            return users
                .SelectFacets<User, UserDto>()
                .ToList();
        }
    }
}
