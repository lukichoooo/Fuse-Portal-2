using FusePortal.Domain.Entities.UserAggregate;
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


        public async Task<User?> GetUserDetailsByIdAsync(Guid id)
            => await _context.Users.FindAsync(id); // add includes later


        public async Task<List<User>> GetUsersPageAsync(Guid? lastId, int pageSize)
        {
            IQueryable<User> query = _context.Users;

            if (lastId is not null)
                query = query.Where(u => u.Id > lastId);

            return await query
                .OrderBy(u => u.Id)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
            => await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();


        public async Task AddAsync(User user)
            => await _context.Users.AddAsync(user);

    }
}
