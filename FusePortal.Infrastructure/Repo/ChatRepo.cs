using FusePortal.Domain.Entities.Convo.ChatAggregate;
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
                .AsNoTracking()
                .ToListAsync();
        }

        public async ValueTask<Chat?> GetChatByIdAsync(Guid chatId, Guid userId)
            => await _context.Chats
                .FirstOrDefaultAsync(c => c.Id == chatId && c.UserId == userId);

        public async Task<List<Message>> GetMessagesPageAsync(
                Guid chatId,
                int? topConutNumber,
                int pageSize,
                Guid userId)
        {
            IQueryable<Message> messageQuery = _context.Messages
                .Where(m => m.ChatId == chatId);

            if (topConutNumber is not null)
                messageQuery = messageQuery.Where(m => m.CountNumber < topConutNumber);

            var messages = await messageQuery
                .Include(m => m.Files)
                .OrderByDescending(m => m.CountNumber)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            messages.Reverse();
            return messages;
        }
    }
}
