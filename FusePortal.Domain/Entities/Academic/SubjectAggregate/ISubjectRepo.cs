namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public interface ISubjectRepo
    {
        Task AddAsync(Subject subject);
        void Remove(Subject subject);

        Task<Subject?> FindById(Guid subjectId, Guid userId);
    }
}
