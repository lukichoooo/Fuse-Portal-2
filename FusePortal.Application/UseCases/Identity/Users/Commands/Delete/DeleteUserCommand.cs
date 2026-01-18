using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Commands.Delete
{
    public record DeleteUserCommand(
            Guid Id
            ) : IRequest;
}
