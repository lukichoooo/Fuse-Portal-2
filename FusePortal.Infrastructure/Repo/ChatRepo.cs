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
            var chat = await _context.Chats
                .FirstOrDefaultAsync(c => c.Id == chatId && c.UserId == userId);

            if (chat is null)
                return null;

            if (firstMsgId is not null)
            {
                await context.Entry(chat)
                    .Collection(c => c.Messages)
                    .Query()
                    .OrderByDescending(m => m.CreatedAt)
                    .Where(m => m.Id > firstMsgId)
                    .Take(pageSize)
                    .Reverse()
                    .LoadAsync();
            }
            else
            {
                await context.Entry(chat)
                    .Collection(c => c.Messages)
                    .Query()
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(pageSize)
                    .Reverse()
                    .LoadAsync();
            }

            return chat;
        }
    }
}
