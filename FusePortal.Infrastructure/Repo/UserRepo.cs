using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Repo
{
    public class UserRepo(AppDbContext context) : IUserRepo
    {
        private readonly AppDbContext _context = context;

        public Task<User?> GetByEmailAsync(string email)
            => _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        public ValueTask<User?> GetByIdAsync(Guid id)
            => _context.Users.FindAsync(id);


        public async Task<User?> GetUserWithUnisByIdAsync(Guid id)
            => await _context.Users
                .Include(u => u.Universities)
                .FirstOrDefaultAsync(u => u.Id == id);


        public async Task<List<User>> GetPageAsync(Guid? lastId, int pageSize)
        {
            IQueryable<User> query = _context.Users;

            if (lastId is not null)
                query = query.Where(u => u.Id > lastId);

            return await query
                .OrderBy(u => u.Id)
                .Take(pageSize)
                .ToListAsync();
        }

        public void Remove(User user)
            => _context.Users.Remove(user);

        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

    }
}
