using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Convo.Chats.Exceptions;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;
using FusePortal.Domain.Entities.Convo.ChatAggregate;

namespace FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage
{
    public class SendMessageCommandHandler : BaseCommandHandler<SendMessageCommand>
    {
        private readonly IChatRepo _repo;
        private readonly IIdentityProvider _identity;
        private readonly IFileRepo _fileRepo;

        public SendMessageCommandHandler(
                IChatRepo repo,
                IIdentityProvider identity,
                IFileRepo fileRepo,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
            _fileRepo = fileRepo;
        }

        protected override async Task ExecuteAsync(SendMessageCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var chat = await _repo.GetChatByIdAsync(command.ChatId, userId)
                ?? throw new ChatNotFoundException($"Chat Not Found With Id={command.ChatId}");

            var message = new Message(command.MessageText, fromUser: true, chat.Id);
            chat.SendMessage(message);

            if (command.FileIds != null)
            {
                foreach (var fileId in command.FileIds)
                {
                    var fileE = await _fileRepo.GetFileByIdAsync(fileId, userId)
                        ?? throw new FileNotFoundException($"File Not Found With Id={fileId}");
                    chat.AttachFileToMessage(message.Id, fileE);
                }
            }

            chat.SendMessage(message);
        }
    }
}
