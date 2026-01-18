using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatsPage
{
    public sealed record GetChatsPageQuery(Guid? LastId, int PageSize)
        : IRequest<List<ChatDto>>;
}
