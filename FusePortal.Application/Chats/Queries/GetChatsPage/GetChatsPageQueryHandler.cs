using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.ChatAggregate;
using MediatR;

namespace FusePortal.Application.Chats.Queries.GetChatsPage
{
    public class GetChatsPageQueryHandler(
            IChatRepo repo,
            ICurrentContext currContext
            )
        : IRequestHandler<GetChatsPageQuery, List<ChatDto>>
    {
        private readonly IChatRepo _repo = repo;
        private readonly ICurrentContext _currContext = currContext;

        public async Task<List<ChatDto>> Handle(
                GetChatsPageQuery request,
                CancellationToken cancellationToken)
        {
            var userId = _currContext.GetCurrentUserId();
            var chats = await _repo.GetAllChatsForUserPageAsync(
                    request.LastId,
                    request.PageSize,
                    userId);

            return chats
                .SelectFacets<Chat, ChatDto>()
                .ToList();
        }
    }
}
