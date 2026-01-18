using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Commands.RemoveUniFromUser
{
    public sealed record RemoveUniFromUserCommand(Guid UniId) : IRequest;
}
