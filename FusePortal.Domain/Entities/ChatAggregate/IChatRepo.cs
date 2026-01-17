using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.ChatAggregate
{
    public interface IChatRepo : IRepository<Chat>
    {
        Task AddAsync(Chat chat);

        ValueTask<Chat?> GetChatByIdAsync(Guid chatId, Guid userId);
        Task<Chat?> GetChatWithMessagesPageAsync(
                Guid chatId,
                Guid? firstMsgId,
                int pageSize,
                Guid userId);

        Task<List<Chat>> GetAllChatsForUserPageAsync(Guid? lastId, int pageSize, Guid userId);
    }
}
