using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Academic.Unis.Exceptions;
using FusePortal.Application.UseCases.Identity.Users.Exceptions;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.UseCases.Identity.Users.Commands.RemoveUniFromUser
{
    public class RemoveUniFromUserCommandHandler : BaseCommandHandler<RemoveUniFromUserCommand>
    {
        private readonly IUserRepo _userRepo;
        private readonly IUniRepo _uniRepo;
        private readonly IIdentityProvider _identity;

        public RemoveUniFromUserCommandHandler(
                IUserRepo userRepo,
                IUniRepo uniRepo,
                IIdentityProvider identity,
                IUnitOfWork uow) : base(uow)
        {
            _userRepo = userRepo;
            _uniRepo = uniRepo;
            _identity = identity;
        }

        protected override async Task ExecuteAsync(RemoveUniFromUserCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _identity.GetCurrentUserId();

            var user = await _userRepo.GetUserWithUnisByIdAsync(userId)
                ?? throw new UserNotFoundException($"User With Id={userId}, Not Found");

            var uni = await _uniRepo.GetByIdAsync(request.UniId)
                ?? throw new UniNotFoundException($"University With Id={request.UniId}, Not Found");

            user.LeaveUniversity(uni);
        }
    }
}
