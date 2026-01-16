using FusePortal.Domain.UserAggregate;

namespace FusePortal.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
