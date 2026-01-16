using AutoFixture;
using FusePortal.Application.Auth.RegisterUser;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.UserAggregate;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using Microsoft.Extensions.Options;
using Moq;


namespace ApplicationTests.AuthTests
{
    [TestFixture]
    public class RegisterUserCommandHandlerTests
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

        private RegisterUserCommandHandler CreateSut(IUserRepo repo)
            => new(repo, _encryptor, _jwt);


        [OneTimeSetUp]
        public void BeforeAll()
        {
            _fix.Behaviors.Add(new OmitOnRecursionBehavior());

            var encryptorOptions = Options.Create(encryptorSettings);
            _encryptor = new Encryptor(encryptorOptions);

            var generatorOptions = Options.Create(jwtSettings);
            _jwt = new JwtTokenGenerator(generatorOptions);
        }


        [Test]
        public async Task RegisterAsync_Success()
        {
            var register = _fix.Create<RegisterUserCommand>();

            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByEmailAsync(register.Email))
                .ReturnsAsync(() => null);
            var auth = CreateSut(mock.Object);

            var res = await auth.Handle(register, _fix.Create<CancellationToken>());

            Assert.That(res, Is.Not.Null);
            mock.Verify(r => r.GetByEmailAsync(register.Email), Times.Once());
        }


        [Test]
        public async Task RegisterAsync_Exists_Throws()
        {
            var register = _fix.Create<RegisterUserCommand>();
            var user = _fix.Create<User>();

            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByEmailAsync(register.Email))
                .ReturnsAsync(() => user);
            var auth = CreateSut(mock.Object);

            Assert.ThrowsAsync<UserAlreadyExistsException>(async () =>
                    await auth.Handle(register, _fix.Create<CancellationToken>()));
        }

    }
}
