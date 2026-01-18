using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatsPage
{
    public class GetChatsPageQueryHandler(
            IChatRepo repo,
            IIdentityProvider identity
            )
        : IRequestHandler<GetChatsPageQuery, List<ChatDto>>
    {
        private readonly IChatRepo _repo = repo;
        private readonly IIdentityProvider _identity = identity;

        public async Task<List<ChatDto>> Handle(
                GetChatsPageQuery request,
                CancellationToken cancellationToken)
        {
            var userId = _identity.GetCurrentUserId();
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
