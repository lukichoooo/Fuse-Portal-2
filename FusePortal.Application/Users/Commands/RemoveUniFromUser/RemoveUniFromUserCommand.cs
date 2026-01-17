using MediatR;

namespace FusePortal.Application.Users.Commands.RemoveUniFromUser
{
    public sealed record RemoveUniFromUserCommand(Guid UniId) : IRequest;
}
