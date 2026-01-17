using MediatR;

namespace FusePortal.Application.Chats.Queries.GetChatWithMessagesPage
{
    public sealed record GetChatWithMessagesQuery(
            Guid ChatId,
            Guid? FirstMsgId,
            int PageSize,
            Guid UserId) : IRequest<ChatWithMessagesDto>;
}
