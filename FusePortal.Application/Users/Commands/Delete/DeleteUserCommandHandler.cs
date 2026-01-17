using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Entities.UserAggregate;

namespace FusePortal.Application.Users.Commands.Delete
{
    public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand>
    {
        private readonly IUserRepo _repo;
        private readonly ICurrentContext _currContext;

        public DeleteUserCommandHandler(
            IUserRepo repo,
            ICurrentContext currContext,
            IUnitOfWork uow)
            : base(uow)
        {
            _repo = repo;
            _currContext = currContext;
        }

        protected override async Task ExecuteAsync(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != _currContext.GetCurrentUserId())
            {
                throw new UnauthorizedAccessException(
                        $"not authorized to delete user with Id={request.Id}");
            }

            var user = await _repo.GetByIdAsync(request.Id)
                ?? throw new UserNotFoundException($"User With Id={request.Id}, Not Found");

            _repo.Remove(user);
        }
    }
}

