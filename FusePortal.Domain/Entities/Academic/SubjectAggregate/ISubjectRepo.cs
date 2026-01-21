namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public interface ISubjectRepo
    {
        Task AddAsync(Subject subject);
        void Remove(Subject subject);

        Task<Subject?> GetByIdAsync(Guid subjectId, Guid userId);
        Task<Subject?> GetByIdWithSyllabusesAsync(Guid subjectId, Guid userId);

        Task<List<Subject>> GetPageAsync(Guid? lastId, int pageSize, Guid userId);
    }
}
