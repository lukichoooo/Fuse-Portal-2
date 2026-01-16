using FusePortal.Domain.Entities.UserAggregate;

namespace FusePortal.Application.Interfaces.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
