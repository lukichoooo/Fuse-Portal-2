using Facet.Extensions;
using FusePortal.Application.Chats.Exceptions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.ChatAggregate;
using MediatR;

namespace FusePortal.Application.Chats.Queries.GetChatWithMessagesPage
{
    public class GetChatWithMessagesQueryHandler(
            IChatRepo repo,
            ICurrentContext currContext)
        : IRequestHandler<GetChatWithMessagesQuery, ChatWithMessagesDto>
    {
        private readonly IChatRepo _repo = repo;
        private readonly ICurrentContext _currContext = currContext;

        public async Task<ChatWithMessagesDto> Handle(
                GetChatWithMessagesQuery request,
                CancellationToken cancellationToken)
        {
            var userId = _currContext.GetCurrentUserId();
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
