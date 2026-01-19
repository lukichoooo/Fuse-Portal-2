using Facet.Extensions;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Convo.Chats.Exceptions;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using MediatR;

namespace FusePortal.Application.UseCases.Convo.Chats.Queries.GetMessagesPage
{
    public class GetMessagesPageQueryHandler(
            IChatRepo repo,
            IIdentityProvider identity)
        : IRequestHandler<GetMessagesPageQuery, List<MessageDto>>
    // TODO: add BaseQueryHandler in SeedWork
    {
        private readonly IChatRepo _repo = repo;
        private readonly IIdentityProvider _identity = identity;

        public async Task<List<MessageDto>> Handle(
                GetMessagesPageQuery request,
                CancellationToken cancellationToken)
        {
            var userId = _identity.GetCurrentUserId();
            var messages = await _repo.GetMessagesPageAsync(
                    request.ChatId,
                    request.TopMsgCountNumber,
                    request.PageSize,
                    userId)
                ?? throw new ChatNotFoundException($"Chat with id={request.ChatId} not found");

            return messages
                .SelectFacets<Message, MessageDto>()
                .ToList();
        }
    }
}
