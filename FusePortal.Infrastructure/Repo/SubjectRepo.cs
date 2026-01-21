using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FusePortal.Infrastructure.Repo
{
    public class SubjectRepo : ISubjectRepo
    {
        private readonly AppDbContext _context;

        public SubjectRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subject subject)
            => await _context.Subjects.AddAsync(subject);

        public async Task<Subject?> GetByIdAsync(Guid subjectId, Guid userId)
            => await _context.Subjects.FirstOrDefaultAsync(
                    s => s.Id == subjectId && s.UserId == userId);

        public async Task<Subject?> GetByIdWithSyllabusesAsync(Guid subjectId, Guid userId)
            => await _context.Subjects
            .Include(s => s.Syllabuses)
            .FirstOrDefaultAsync(s => s.Id == subjectId && s.UserId == userId);

        public Task<List<Subject>> GetPageAsync(Guid? lastId, int pageSize, Guid userId)
        {
            IQueryable<Subject> query = _context.Subjects
                .Include(s => s.Syllabuses)
                .Include(s => s.Schedules)
                .Include(s => s.Lecturers)
                .Where(s => s.UserId == userId);

            if (lastId is not null)
                query = query.Where(s => s.Id > lastId);

            return query
                .OrderBy(s => s.Id)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public void Remove(Subject subject)
            => _context.Remove(subject);
    }
}
