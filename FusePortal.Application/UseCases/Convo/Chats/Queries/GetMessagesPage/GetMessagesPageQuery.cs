using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetMessagesPage
{
    public sealed record GetMessagesPageQuery(
            Guid ChatId,
            int? TopMsgCountNumber,
            int PageSize) : IRequest<List<MessageDto>>;
}
