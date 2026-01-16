using MediatR;

namespace FusePortal.Application.Auth.LoginUser
{
    public sealed record LoginUserCommand(LoginRequest Login) : IRequest<AuthResponse>;
}
