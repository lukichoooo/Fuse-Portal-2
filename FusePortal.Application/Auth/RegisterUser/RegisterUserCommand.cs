using FusePortal.Domain.Common.ValueObjects.Address;
using MediatR;

namespace FusePortal.Application.Auth.RegisterUser
{
    public sealed record RegisterUserCommand(
            string Name,
            string Email,
            string Password,
            Address Address)
        : IRequest<AuthResponse>;
}
