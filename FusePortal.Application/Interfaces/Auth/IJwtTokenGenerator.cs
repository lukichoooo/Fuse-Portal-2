using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.Interfaces.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
