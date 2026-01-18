using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Identity.Users.Exceptions;
using FusePortal.Domain.Common.ValueObjects.Address;
using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.UseCases.Identity.Users.Commands.Update
{
    public class UpdateUserCommandHandler : BaseCommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepo _repo;
        private readonly IIdentityProvider _identity;
        private readonly IUserSecurityService _userSecurity;

        public UpdateUserCommandHandler(
            IUserRepo repo,
            IIdentityProvider identity,
            IUserSecurityService userSecurity,
            IUnitOfWork uow)
            : base(uow)
        {
            _repo = repo;
            _identity = identity;
            _userSecurity = userSecurity;
        }

        protected override async Task ExecuteAsync(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _identity.GetCurrentUserId();

            var user = await _repo.GetByIdAsync(userId)
                ?? throw new UserNotFoundException($"User With Id={userId}, Not Found");

            _userSecurity.VerifyPassword(user, request.CurrentPassword);

            var addrDto = request.Address;
            user.UpdateEmail(request.Email);
            user.ChangeAddress(new Address(country: addrDto.Country, city: addrDto.City));
            var newPassHash = _userSecurity.HashPassword(request.NewPassword);
            user.UpdatePasswordHash(newPassHash);
        }
    }
}
