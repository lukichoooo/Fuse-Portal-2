using MediatR;

namespace FusePortal.Application.Users.Commands.Delete
{
    public record DeleteUserCommand(
            Guid Id
            ) : IRequest;
}
