using FusePortal.Domain.UserAggregate;

namespace FusePortal.Application.Interfaces.Auth
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
