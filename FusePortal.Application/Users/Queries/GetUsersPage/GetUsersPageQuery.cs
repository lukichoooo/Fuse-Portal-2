using MediatR;

namespace FusePortal.Application.Users.Queries.GetUsersPage
{
    public record GetUsersPageQuery(
            Guid? LastId,
            int PageSize) : IRequest<List<UserDto>>;
}
