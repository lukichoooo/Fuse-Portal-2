using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.UniversityAggregate
{
    public interface IUniRepo : IRepository<University>
    {
        ValueTask<University?> GetByIdAsync(Guid id);
        Task<University?> GetByNameAsync(string name);
        Task AddAsync(University uni);
        void Remove(University uni);

        Task<List<University>> GetPageAsync(Guid? lastId, int pageSize);
    }
}
