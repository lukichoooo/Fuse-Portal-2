using FusePortal.Application.Common;
using MediatR;

namespace FusePortal.Application.Auth.RegisterUser
{
    public sealed record RegisterUserCommand(
            string Name,
            string Email,
            string Password,
            AddressDto Address)
        : IRequest<AuthResponse>;
}
