using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.Entities.UserAggregate;

namespace FusePortal.Application.Users.Commands.Update
{
    public class UpdateUserCommandHandler : BaseCommandHandler<UpdateUserCommand>
    {
        private readonly IUserRepo _repo;
        private readonly ICurrentContext _currContext;
        private readonly IUserSecurityService _userSecurity;

        public UpdateUserCommandHandler(
            IUserRepo repo,
            ICurrentContext currContext,
            IUserSecurityService userSecurity,
            IUnitOfWork uow)
            : base(uow)
        {
            _repo = repo;
            _currContext = currContext;
            _userSecurity = userSecurity;
        }

        protected override async Task ExecuteAsync(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currContext.GetCurrentUserId();

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
