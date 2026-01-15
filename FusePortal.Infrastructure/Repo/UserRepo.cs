using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Repo
{
    public class UserRepo(AppDbContext context) : IUserRepo
    {
        private readonly AppDbContext _context = context;

        public Task<User?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }


        public ValueTask<User?> GetByIdAsync(int id)
            => _context.Users.FindAsync(id);

        public async Task<List<User>> GetUsersPageAsync(int? lastId, int pageSize)
        {
            IQueryable<User> query = _context.Users;

            if (lastId is not null)
                query = query.Where(u => u.Id > lastId);

            return await query
                .OrderBy(u => u.Id)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteByIdAsync(int id)
            => await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();


        public async Task<User?> GetUserDetailsByIdAsync(int id)
            => await _context.Users.FindAsync(id); // add includes later


        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }


    }
}
