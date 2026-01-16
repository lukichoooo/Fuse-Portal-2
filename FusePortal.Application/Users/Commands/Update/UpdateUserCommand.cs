using FusePortal.Application.Common;
using MediatR;

namespace FusePortal.Application.Users.Commands.Update
{
    public record UpdateUserCommand(
            Guid Id,
            string Email,
            AddressDto Address,
            string Password
            ) : IRequest<UserDto>;
}
