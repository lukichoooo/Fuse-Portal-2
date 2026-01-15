using FusePortal.Application.Common;
using MediatR;

namespace FusePortal.Application.Users.Commands.Update
{
    public record UpdateUserCommand(
            int Id,
            string Name,
            string Email,
            AddressDto Address,
            string Password
            ) : IRequest<UserDto>;
}
