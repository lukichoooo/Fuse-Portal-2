using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Commands.AddUniToUser
{
    public sealed record AddUniToUserCommand(Guid UniId) : IRequest;
}
