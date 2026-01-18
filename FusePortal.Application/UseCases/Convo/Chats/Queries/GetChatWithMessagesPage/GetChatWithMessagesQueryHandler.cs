using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Convo.Chats.Exceptions;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatWithMessagesPage
{
    public class GetChatWithMessagesQueryHandler(
            IChatRepo repo,
            IIdentityProvider identity)
        : IRequestHandler<GetChatWithMessagesQuery, ChatWithMessagesDto>
    // TODO: add BaseQueryHandler in SeedWork
    {
        private readonly IChatRepo _repo = repo;
        private readonly IIdentityProvider _identity = identity;

        public async Task<ChatWithMessagesDto> Handle(
                GetChatWithMessagesQuery request,
                CancellationToken cancellationToken)
        {
            var userId = _identity.GetCurrentUserId();
            var chat = await _repo.GetChatWithMessagesPageAsync(
                    request.ChatId,
                    request.FirstMsgId,
                    request.PageSize,
                    userId)
                ?? throw new ChatNotFoundException($"Chat with id={request.ChatId} not found");

            return chat.ToFacet<Chat, ChatWithMessagesDto>();
        }
    }
}
