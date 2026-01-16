using MediatR;

namespace FusePortal.Application.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(
            Guid Id
            ) : IRequest<UserDetailsDto>;
}
