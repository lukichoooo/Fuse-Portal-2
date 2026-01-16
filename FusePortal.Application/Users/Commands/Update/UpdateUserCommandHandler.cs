using Facet.Extensions;
using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Commands.Update
{
    public class UpdateUserCommandHandler(
            IUserRepo repo,
            ICurrentContext currContext,
            IUserSecurityService userSecurity)
        : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepo _repo = repo;
        private readonly ICurrentContext _currContext = currContext;
        private readonly IUserSecurityService _userSecurity = userSecurity;

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != _currContext.GetCurrentUserId())
            {
                throw new UnauthorizedAccessException(
                        $"not authorized to update user with Id={request.Id}");
            }

            var user = await _repo.GetByIdAsync(request.Id)
                ?? throw new UserNotFoundException($"User With Id={request.Id}, Not Found");

            _userSecurity.VerifyPassword(user, request.Password);

            user.UpdateEmail(request.Email);
            user.ChangeAddress(request.Address.ToSource<AddressDto, Address>());

            await _repo.UpdateAsync(user);
            return user.ToFacet<User, UserDto>();
        }
    }
}
