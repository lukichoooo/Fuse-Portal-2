
namespace FusePortal.Application.Auth
{
    public record AuthResponse(
            string AccessToken,
            string RefreshToken
            );
}
