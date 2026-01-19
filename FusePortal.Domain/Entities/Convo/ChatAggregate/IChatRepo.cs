using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Convo.ChatAggregate
{
    public interface IChatRepo : IRepository<Chat>
    {
        Task AddAsync(Chat chat);

        ValueTask<Chat?> GetChatByIdAsync(Guid chatId, Guid userId);

        Task<List<Message>> GetMessagesPageAsync(
                Guid chatId,
                int? topMsgCountNumber,
                int pageSize,
                Guid userId);

        Task<List<Chat>> GetAllChatsForUserPageAsync(Guid? lastId, int pageSize, Guid userId);
    }
}
