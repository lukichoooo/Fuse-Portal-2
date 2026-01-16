using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoFixture;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using Microsoft.Extensions.Options;

namespace InfrastructureTests.AuthTests
{
    [TestFixture]
    public class JwtTokenGeneratorTests
    {
        private IJwtTokenGenerator _jwt;
        private readonly Fixture _fix = new();

        private readonly JwtSettings _settings = new()
        {
            Key = "asdadadaisodASifimallivewhyareumyremedyosajfa",
            Issuer = "lukaco",
            Audience = "tqven",
            AccessTokenExpiration = 5,
            RefreshTokenExpiration = 30
        };

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _fix.Behaviors.Add(new OmitOnRecursionBehavior());

            var options = Options.Create(_settings);
            _jwt = new JwtTokenGenerator(options);
        }

        [Test]
        public void GenerateToken_Returns_Valid_Result()
        {
            var user = _fix.Create<User>();
            var handler = new JwtSecurityTokenHandler();

            var tokenString = _jwt.GenerateToken(user);
            var token = handler.ReadJwtToken(tokenString);
            var claims = token.Claims;

            var id = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;

            Assert.That(id, Is.EqualTo(user.Id));
            Assert.That(email, Is.EqualTo(user.Email));
            Assert.That(role, Is.EqualTo(user.Role.ToString()));
        }
    }
}
