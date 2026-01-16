using AutoFixture;
using FusePortal.Application.Auth.LoginUser;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Common.ValueObjects;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using Microsoft.Extensions.Options;
using Moq;

namespace ApplicationTests.AuthTests
{
    [TestFixture]
    public class LoginUserCommandHandlerTests
    {
        private readonly IUserRepo _repo;

        private IEncryptor _encryptor;
        private IJwtTokenGenerator _jwt;

        private readonly Fixture _fix = new();


        private readonly JwtSettings jwtSettings = new()
        {
            Key = "asdadadaisodASifimallivewhyareumyremedyosajfa",
            Issuer = "lukaco",
            Audience = "tqven",
            AccessTokenExpiration = 5,
            RefreshTokenExpiration = 30
        };
        private readonly EncryptorSettings encryptorSettings = new()
        {
            Key = Convert.FromBase64String("MDEyMzQ1Njc4OWFiY2RlZjAxMjM0NTY3ODlhYmNkZWY="),
            IV = Convert.FromBase64String("bXlJbml1VmVjdG9yMTIzNA==")
        };


        [OneTimeSetUp]
        public void BeforeAll()
        {
            _fix.Behaviors.Add(new OmitOnRecursionBehavior());

            var encryptorOptions = Options.Create(encryptorSettings);
            _encryptor = new Encryptor(encryptorOptions);

            var generatorOptions = Options.Create(jwtSettings);
            _jwt = new JwtTokenGenerator(generatorOptions);
        }

        private LoginUserCommandHandler CreateSut(IUserRepo repo)
            => new(repo, _encryptor, _jwt);

        [Test]
        public async Task LoginAsync_Success()
        {
            var plainPassword = "Test1234!";
            var encryptedPassword = _encryptor.Encrypt(plainPassword);

            var user = _fix.Build<User>()
                .FromFactory(() => new User(
                    name: "idk",
                    email: "user@example.com",
                    passwordHash: encryptedPassword,
                    address: _fix.Create<Address>()
                ))
                .Create();

            var login = new LoginUserCommand(user.Email, plainPassword);


            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByEmailAsync(login.Email))
                .ReturnsAsync(() => user);
            var auth = CreateSut(mock.Object);

            var res = await auth.Handle(login, _fix.Create<CancellationToken>());

            Assert.That(res, Is.Not.Null);
            mock.Verify(r => r.GetByEmailAsync(login.Email), Times.Once());
        }


        [Test]
        public async Task LoginAsync_NotFound_Throws()
        {
            var login = _fix.Create<LoginUserCommand>();

            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByEmailAsync(login.Email))
                .ReturnsAsync(() => null);
            var auth = CreateSut(mock.Object);

            Assert.ThrowsAsync<UserNotFoundException>(async () =>
                    await auth.Handle(login, _fix.Create<CancellationToken>()));
            mock.Verify(r => r.GetByEmailAsync(login.Email), Times.Once());
        }


        [Test]
        public async Task LoginAsync_WrongPassword_Throws()
        {
            var user = _fix.Create<User>();
            var login = new LoginUserCommand(user.Email, user.PasswordHash);

            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByEmailAsync(login.Email))
                .ReturnsAsync(() => user);
            var auth = CreateSut(mock.Object);

            Assert.ThrowsAsync<UserWrongCredentialsException>(async () =>
                    await auth.Handle(login, _fix.Create<CancellationToken>()));
            mock.Verify(r => r.GetByEmailAsync(login.Email), Times.Once());
        }
    }
}
