using FusePortal.Domain.Entities.UniversityAggregate;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Repo
{
    public class UniRepo(AppDbContext context) : IUniRepo
    {
        private readonly AppDbContext _context = context;

        public ValueTask<University?> GetByIdAsync(Guid id)
            => _context.Universities.FindAsync(id);

        public Task<University?> GetByNameAsync(string name)
            => _context.Universities.FirstOrDefaultAsync(x => x.Name == name);

        public async Task AddAsync(University uni)
            => await _context.Universities.AddAsync(uni);

        public void Remove(University uni)
            => _context.Universities.Remove(uni);

        public async Task<List<University>> GetPageAsync(Guid? lastId, int pageSize)
        {
            IQueryable<University> query = _context.Universities;

            if (lastId is not null)
                query = query.Where(u => u.Id > lastId);

            return await query
                .OrderBy(u => u.Id)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
