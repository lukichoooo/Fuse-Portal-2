namespace FusePortal.Application.UseCases.Identity.Auth
{
    public record AuthResponse(
            string AccessToken,
            string RefreshToken
            );
}
