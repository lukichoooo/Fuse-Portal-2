using FusePortal.Domain.Common.ValueObjects.Address;
using MediatR;

namespace FusePortal.Application.Users.Commands.Update
{
    public record UpdateUserCommand(
            string Email,
            Address Address,
            string CurrentPassword,
            string NewPassword
            ) : IRequest;
}
