using MediatR;

namespace FusePortal.Application.Auth.RegisterUser
{
    public sealed record RegisterUserCommand(RegisterRequest Register)
        : IRequest<AuthResponse>;
}
