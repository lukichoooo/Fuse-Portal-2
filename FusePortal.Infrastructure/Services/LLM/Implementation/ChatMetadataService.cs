using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.Services;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using FusePortal.Infrastructure.Services.LLM.Interfaces;

namespace FusePortal.Infrastructure.Services.LLM.Implementation
{
    public class ChatMetadataService(
            IChatRepo repo,
            ICache cache,
            IIdentityProvider currentContext
            ) : IChatMetadataService
    {
        private readonly ICache _cache = cache;
        private readonly IChatRepo _chatRepo = repo;
        private readonly IIdentityProvider _identity = currentContext;


        public async Task<string?> GetLastResponseIdAsync(Guid chatId)
        {
            Guid userId = _identity.GetCurrentUserId();
            string key = chatId.ToString() + userId.ToString();

            return await _cache.GetValueAsync(key)
                ?? (await _chatRepo.GetChatByIdAsync(chatId, userId))?.LastResponseId;
        }


        public Task SetLastResponseIdAsync(Guid chatId, string responseId)
        {
            Guid userId = _identity.GetCurrentUserId();
            string key = chatId.ToString() + userId.ToString();

            async Task dbUpdateTask()
            {
                var chat = await _chatRepo.GetChatByIdAsync(chatId, userId);
                chat?.UpdateLastResponseId(responseId);
            }

            return Task.WhenAll
            (
                dbUpdateTask(),
                _cache.SetValueAsync(key, responseId)
            );
        }
    }
}
