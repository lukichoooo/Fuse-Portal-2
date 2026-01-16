using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FusePortal.Application.Interfaces;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Settings.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace FusePortal.Infrastructure.Authenticatoin
{
    public class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings = options.Value;

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims:
                [
                    new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("role", user.Role.ToString())
                ],
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
