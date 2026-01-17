using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.UserAggregate
{
    public interface IUserRepo : IRepository<User>
    {
        Task AddAsync(User user);
        void Remove(User user);

        Task<User?> GetUserWithUnisByIdAsync(Guid id);
        ValueTask<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetPageAsync(Guid? lastId, int pageSize);
    }
}

