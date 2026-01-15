using MediatR;

namespace FusePortal.Application.Users.Commands.Delete
{
    public record DeleteUserCommand(int Id) : IRequest<int>;
}
