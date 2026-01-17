using FusePortal.Domain.Entities.ChatAggregate;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Repo
{
    public class ChatRepo(AppDbContext context) : IChatRepo
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(Chat chat)
            => await _context.Chats.AddAsync(chat);


        public Task<List<Chat>> GetAllChatsForUserPageAsync(
                Guid? lastChatId,
                int pageSize,
                Guid userId)
        {
            IQueryable<Chat> chats = _context.Chats
                .Where(c => c.UserId == userId);

            if (lastChatId is not null)
            {
                chats = chats
                    .Where(c => c.Id > lastChatId);
            }

            return chats
                .OrderBy(c => c.Id)
                .Take(pageSize)
                .ToListAsync();
        }

        public async ValueTask<Chat?> GetChatByIdAsync(Guid chatId, Guid userId)
            => await _context.Chats
                .FirstOrDefaultAsync(c => c.Id == chatId && c.UserId == userId);

        public async Task<Chat?> GetChatWithMessagesPageAsync(
                Guid chatId,
                Guid? firstMsgId,
                int pageSize,
                Guid userId)
        {
            if (firstMsgId is null)
            {
                return await _context.Chats.Include(c => c.Messages
                                    .OrderByDescending(m => m.Id)
                                    .Take(pageSize)
                                    .OrderByDescending(m => m.CreatedAt))
                    .ThenInclude(m => m.Files)
                    .FirstOrDefaultAsync();
            }

            return await _context.Chats.Include(c => c.Messages
                           .Where(m => m.Id > firstMsgId)
                                .OrderByDescending(m => m.Id)
                                .Take(pageSize)
                                .OrderByDescending(m => m.CreatedAt))
                           .ThenInclude(m => m.Files)
                           .FirstOrDefaultAsync();
        }
    }
}
