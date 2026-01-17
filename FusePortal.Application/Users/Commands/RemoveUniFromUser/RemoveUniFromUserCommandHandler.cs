using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Unis.Exceptions;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Entities.UniversityAggregate;
using FusePortal.Domain.Entities.UserAggregate;

namespace FusePortal.Application.Users.Commands.RemoveUniFromUser
{
    public class RemoveUniFromUserCommandHandler : BaseCommandHandler<RemoveUniFromUserCommand>
    {
        private readonly IUserRepo _userRepo;
        private readonly IUniRepo _uniRepo;
        private readonly ICurrentContext _currContext;

        public RemoveUniFromUserCommandHandler(
                IUserRepo userRepo,
                IUniRepo uniRepo,
                ICurrentContext currContext,
                IUnitOfWork uow) : base(uow)
        {
            _userRepo = userRepo;
            _uniRepo = uniRepo;
            _currContext = currContext;
        }

        protected override async Task ExecuteAsync(RemoveUniFromUserCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _currContext.GetCurrentUserId();

            var user = await _userRepo.GetUserWithUnisByIdAsync(userId)
                ?? throw new UserNotFoundException($"User With Id={userId}, Not Found");

            var uni = await _uniRepo.GetByIdAsync(request.UniId)
                ?? throw new UniNotFoundException($"University With Id={request.UniId}, Not Found");

            user.RemoveUniversity(uni);
        }
    }
}
