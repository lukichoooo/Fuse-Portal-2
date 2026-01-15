using FusePortal.Domain.UserAggregate;
using MediatR;

namespace FusePortal.Application.Users.Commands.Delete
{
    public class DeleteUserCommandHandler(IUserRepo repo)
        : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IUserRepo _repo = repo;

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            => await _repo.DeleteByIdAsync(request.Id);
    }
}
