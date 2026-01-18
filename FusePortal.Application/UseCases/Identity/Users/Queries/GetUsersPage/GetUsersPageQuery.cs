using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Queries.GetUsersPage
{
    public record GetUsersPageQuery(
            Guid? LastId,
            int PageSize) : IRequest<List<UserDto>>;
}
