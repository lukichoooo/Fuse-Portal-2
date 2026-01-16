using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Commands.Delete
{
    public class DeleteUserCommandHandler(IUserRepo repo, ICurrentContext currContext)
        : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserRepo _repo = repo;
        private readonly ICurrentContext _currContext = currContext;

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Id != _currContext.GetCurrentUserId())
            {
                throw new UnauthorizedAccessException(
                        $"not authorized to delete user with Id={request.Id}");
            }

            return await _repo.DeleteByIdAsync(request.Id);
        }
    }
}

