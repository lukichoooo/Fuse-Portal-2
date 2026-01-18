using FusePortal.Domain.Common.ValueObjects.Address;
using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Commands.Update
{
    public record UpdateUserCommand(
            string Email,
            Address Address,
            string CurrentPassword,
            string NewPassword
            ) : IRequest;
}
