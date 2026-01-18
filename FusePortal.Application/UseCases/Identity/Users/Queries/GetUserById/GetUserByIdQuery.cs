using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(
            Guid Id
            ) : IRequest<UserDetailsDto>;
}
