using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.UserAggregate
{
    public interface IUserRepo : IRepository<User>
    {
        Task<User?> GetUserDetailsByIdAsync(Guid id);
        ValueTask<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task DeleteByIdAsync(Guid id);
        Task<List<User>> GetUsersPageAsync(Guid? lastId, int pageSize);
    }
}

