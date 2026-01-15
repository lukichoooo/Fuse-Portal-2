using FusePortal.Domain.Entities;

namespace FusePortal.Domain.Repo
{
    public interface IUserRepo
    {
        Task<User?> GetUserDetailsByIdAsync(int id);
        ValueTask<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateUserCredentialsAsync(User user);
        Task<User> DeleteByIdAsync(int id);

        Task<List<User>> GetAllPageAsync(int? lastId, int pageSize);
    }
}
