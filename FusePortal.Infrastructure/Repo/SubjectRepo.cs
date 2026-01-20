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

        public async Task<Subject?> FindById(Guid subjectId, Guid userId)
            => await _context.Subjects.FirstOrDefaultAsync(
                    s => s.Id == subjectId && s.UserId == userId);

        public void Remove(Subject subject)
            => _context.Remove(subject);
    }
}
