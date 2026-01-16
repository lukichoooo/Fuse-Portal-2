using MediatR;

namespace FusePortal.Application.Auth.LoginUser
{
    public sealed record LoginUserCommand(
            string Email,
            string Password) : IRequest<AuthResponse>;
}
