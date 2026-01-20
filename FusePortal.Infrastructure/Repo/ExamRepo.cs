using FusePortal.Domain.Entities.Academic.ExamAggregate;
using FusePortal.Infrastructure.Data;

namespace FusePortal.Infrastructure.Repo
{
    public class ExamRepo : IExamRepo
    {
        private readonly AppDbContext _context;

        public ExamRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Exam exam)
            => await _context.AddAsync(exam);

        public async Task<Exam?> FindById(Guid examId)
            => await _context.Exams.FindAsync(examId);

        public void Remove(Exam exam)
            => _context.Exams.Remove(exam);
    }
}
