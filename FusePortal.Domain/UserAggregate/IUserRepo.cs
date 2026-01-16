using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.UserAggregate;

public interface IUserRepo : IRepository<User>
{
    Task<User?> GetUserDetailsByIdAsync(int id);
    ValueTask<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<int> DeleteByIdAsync(int id);

    Task<List<User>> GetUsersPageAsync(int? lastId, int pageSize);
}
