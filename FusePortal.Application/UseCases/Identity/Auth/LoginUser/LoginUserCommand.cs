using MediatR;

namespace FusePortal.Application.UseCases.Identity.Auth.LoginUser
{
    public sealed record LoginUserCommand(
            string Email,
            string Password) : IRequest<AuthResponse>;
}
