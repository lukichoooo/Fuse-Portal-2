using MediatR;

namespace FusePortal.Application.Users.Queries.GetUsersPage
{
    public record GetUsersPageQuery(int? LastId, int PageSize) : IRequest<List<UserDto>> { }
}
