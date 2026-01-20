using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate
{
    public interface IExamRepo : IRepository<Exam>
    {
        Task AddAsync(Exam exam);
        void Remove(Exam exam);

        Task<Exam?> FindById(Guid examId);
    }
}
