using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Commands.Delete
{
    public class DeleteUserCommandHandler(
            IUserRepo repo,
            ICurrentContext currContext,
            IUnitOfWork uow)
        : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepo _repo = repo;
        private readonly ICurrentContext _currContext = currContext;
        private readonly IUnitOfWork _uow = uow;

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != _currContext.GetCurrentUserId())
            {
                throw new UnauthorizedAccessException(
                        $"not authorized to delete user with Id={request.Id}");
            }

            await _repo.DeleteByIdAsync(request.Id);
            await _uow.CommitAsync();
        }
    }
}

