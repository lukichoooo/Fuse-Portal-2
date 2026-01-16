using MediatR;

namespace FusePortal.Application.Users.Commands.Create
{
    public record CreateUserCommand(UserCreateCommandDto Dto) : IRequest<UserDto>;
}
