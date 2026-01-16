namespace FusePortal.Infrastructure.Settings.Auth
{
    public record JwtSettings
    {
        public string Key { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public int AccessTokenExpiration { get; init; }
        public int RefreshTokenExpiration { get; init; }
    }
}
