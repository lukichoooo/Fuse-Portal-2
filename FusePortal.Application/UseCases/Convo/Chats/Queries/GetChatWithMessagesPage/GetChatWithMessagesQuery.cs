using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatWithMessagesPage
{
    public sealed record GetChatWithMessagesQuery(
            Guid ChatId,
            Guid? FirstMsgId,
            int PageSize) : IRequest<ChatWithMessagesDto>;
}
