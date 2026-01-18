using Facet.Extensions;
using FusePortal.Application.Common;
using FusePortal.Application.Common.SeedWork;
using FusePortal.Application.Events.IntergrationEvents;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.Services;
using FusePortal.Application.UseCases.Convo.Chats;
using FusePortal.Application.UseCases.Convo.Chats.Exceptions;
using FusePortal.Domain.Entities.Convo.ChatAggregate;

namespace FusePortal.Application.Events.Handlers.IntergrationEvent
{
    public class ChatMessageSentIntergrationEventHandler :
        BaseIntergrationEventHandler<ChatMessageSentIntergrationEvent>
    {
        private readonly IChatRepo _repo;
        private readonly ILLMMessageService _llm;
        private readonly IIdentityProvider _identity;
        private readonly IMessageStreamer _streamer;

        public ChatMessageSentIntergrationEventHandler(
                IChatRepo repo,
                ILLMMessageService llm,
                IIdentityProvider identity,
                IMessageStreamer streamer,
                IUnitOfWork uow) : base(uow)
        {
            _repo = repo;
            _llm = llm;
            _identity = identity;
            _streamer = streamer;
        }

        protected override async Task ExecuteAsync(
                ChatMessageSentIntergrationEvent evt,
                CancellationToken ct)
        {
            var userId = _identity.GetCurrentUserId();
            var chat = await _repo.GetChatByIdAsync(evt.ChatId, userId)
                ?? throw new ChatNotFoundException($"Chat with id={evt.ChatId} not found");

            MessageLLMDto message = chat.Messages
                .First(x => x.Id == evt.MessageId)
                .ToFacet<Message, MessageLLMDto>();

            MessageLLMDto responseDto;
            if (evt.Streaming)
            {
                responseDto = await _llm.SendMessageStreamingAsync(
                        message,
                        (chunk) => _streamer.StreamAsync(chat.Id, chunk),
                        ct);
            }
            else
            {
                responseDto = await _llm.SendMessageAsync(message, ct);
            }

            var response = new Message(responseDto.Text, fromUser: false, chat.Id);
            chat.RecieveResponse(response);
        }
    }
}
