using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.Interfaces.Auth
{
    public interface IUserSecurityService
    {
        void VerifyPassword(User user, string password);
        string HashPassword(string password);
    }
}
