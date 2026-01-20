using Facet.Extensions;
using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.Services;
using FusePortal.Application.Interfaces.Services.File;
using FusePortal.Application.UseCases.Convo.Chats.Exceptions;
using FusePortal.Domain.Common.Objects;
using FusePortal.Domain.Entities.Convo.ChatAggregate;

namespace FusePortal.Application.UseCases.Convo.Chats.Commands.SendMessage
{
    public class SendMessageCommandHandler : BaseCommandHandler<SendMessageCommand>
    {
        private readonly IChatRepo _repo;
        private readonly IIdentityProvider _identity;
        private readonly IFileProcessor _fileProcessor;
        private readonly ILLMMessageService _llm;
        private readonly IMessageStreamer _streamer;

        public SendMessageCommandHandler(
                IChatRepo repo,
                IIdentityProvider identity,
                IFileProcessor fileProcessor,
                ILLMMessageService llm,
                IMessageStreamer streamer,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _identity = identity;
            _fileProcessor = fileProcessor;
            _llm = llm;
            _streamer = streamer;
        }

        protected override async Task ExecuteAsync(SendMessageCommand command, CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var chat = await _repo.GetChatByIdAsync(command.ChatId, userId)
                ?? throw new ChatNotFoundException($"Chat Not Found With Id={command.ChatId}");

            List<FileData> files = await _fileProcessor.ProcessFilesAsync(command.FileUploads);
            chat.SendMessage(command.MessageText, files);

            MessageLLMDto llmMessage = chat.GetLastMessage()
                                .ToFacet<Message, MessageLLMDto>();

            MessageLLMDto responseDto;
            if (command.Streaming)
            {
                responseDto = await _llm.SendMessageStreamingAsync(
                        llmMessage,
                        (chunk) => _streamer.StreamAsync(chat.Id, chunk, ct),
                        ct);
            }
            else
            {
                responseDto = await _llm.SendMessageAsync(llmMessage, ct);
            }

            chat.RecieveResponse(responseDto.Text);
        }
    }
}
