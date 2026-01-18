using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.Convo.ChatAggregate;

namespace FusePortal.Application.UseCases.Convo.Chats.Commands.CreateChat
{
    public class CreateChatCommandHandler : BaseCommandHandler<CreateChatCommand>
    {
        private readonly IChatRepo _repo;
        private readonly IIdentityProvider _currentContext;

        public CreateChatCommandHandler(
                IChatRepo repo,
                IIdentityProvider currentContext,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _currentContext = currentContext;
        }

        protected override async Task ExecuteAsync(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentContext.GetCurrentUserId();
            var chat = new Chat(request.Name, userId);
            await _repo.AddAsync(chat);
        }
    }
}
