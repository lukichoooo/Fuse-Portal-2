using MediatR;

namespace FusePortal.Application.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<UserDetailsDto> { }
}
