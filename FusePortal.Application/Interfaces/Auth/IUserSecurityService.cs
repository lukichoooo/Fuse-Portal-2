using FusePortal.Domain.Entities.UserAggregate;

namespace FusePortal.Application.Interfaces.Auth
{
    public interface IUserSecurityService
    {
        void VerifyPassword(User user, string password);
    }
}
