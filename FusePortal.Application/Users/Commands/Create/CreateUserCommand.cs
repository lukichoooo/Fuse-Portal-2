using MediatR;

namespace FusePortal.Application.Users.Commands.Create
{
    public record CreateUserCommand(UserCreateDto Dto) : IRequest<UserDto>;
}
