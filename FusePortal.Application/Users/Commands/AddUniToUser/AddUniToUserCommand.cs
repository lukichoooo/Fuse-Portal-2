using MediatR;

namespace FusePortal.Application.Users.Commands.AddUniToUser
{
    public sealed record AddUniToUserCommand(Guid UniId) : IRequest;
}
