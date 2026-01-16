namespace FusePortal.Domain.Entities.UniversityAggregate
{
    public interface IUniRepo
    {
        Task<University?> GetByIdAsync(Guid id);
        Task<University?> GetByNameAsync(string name);
        Task AddAsync(University university);
        Task DeleteByIdAsync(Guid id);

        Task<List<University>> GetPageAsync(Guid? lastId, int pageSize);
    }
}
