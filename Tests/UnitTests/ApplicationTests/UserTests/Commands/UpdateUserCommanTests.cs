using AutoFixture;
using Facet.Extensions;
using FusePortal.Application.Common;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Users.Commands.Update;
using FusePortal.Application.Users.Exceptions;
using FusePortal.Domain.Common.ValueObjects.Address;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Infrastructure.Auth;
using FusePortal.Infrastructure.Settings.Auth;
using Microsoft.Extensions.Options;
using Moq;

namespace ApplicationTests.UserTests.Commands
{
    [TestFixture]
    public class UpdateUserCommandTests
    {
        private IEncryptor _encryptor;
        private IUserSecurityService _security;
        private IUnitOfWork _uow;

        private readonly Fixture _fix = new();

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


            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(x => x.CommitAsync());
            _uow = uowMock.Object;


            _security = new UserSecurityService(_encryptor);
        }

        private UpdateUserCommandHandler CreateSut(
                IUserRepo repo,
                IIdentityProvider identity)
            => new(repo, identity, _security, _uow);

        [Test]
        public async Task Handle_Success()
        {
            var currPassword = "Test1234!";
            var encryptedCurrPassword = _encryptor.Encrypt(currPassword);
            var newPasword = "newPeiasajd!";

            var user = _fix.Build<User>()
                .FromFactory(() => new User(
                    name: "idk",
                    email: "user@example.com",
                    passwordHash: encryptedCurrPassword,
                    address: _fix.Create<Address>()
                ))
                .Create();

            var update = new UpdateUserCommand(
                    Email: user.Email,
                    Address: user.Address,
                    CurrentPassword: currPassword,
                    NewPassword: newPasword);


            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByIdAsync(user.Id))
                .ReturnsAsync(() => user);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(x => x.GetCurrentUserId())
                .Returns(user.Id);

            var sut = CreateSut(mock.Object, identityMock.Object);

            await sut.Handle(update, _fix.Create<CancellationToken>());

            Assert.That(user.PasswordHash, Is.EqualTo(_encryptor.Encrypt(newPasword)));
            mock.Verify(r => r.GetByIdAsync(user.Id), Times.Once());
        }


        [Test]
        public async Task Handle_UserNotFound()
        {
            var currPassword = "Test1234!";
            var encryptedCurrPassword = _encryptor.Encrypt(currPassword);
            var newPasword = "newPeiasajd!";

            var user = _fix.Build<User>()
                .FromFactory(() => new User(
                    name: "idk",
                    email: "user@example.com",
                    passwordHash: encryptedCurrPassword,
                    address: _fix.Create<Address>()
                ))
                .Create();

            var update = new UpdateUserCommand(
                    Email: user.Email,
                    Address: user.Address,
                    CurrentPassword: currPassword,
                    NewPassword: newPasword);


            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByIdAsync(user.Id))
                .ReturnsAsync(() => null);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(x => x.GetCurrentUserId())
                .Returns(user.Id);

            var sut = CreateSut(mock.Object, identityMock.Object);

            Assert.ThrowsAsync<UserNotFoundException>(async () =>
                    await sut.Handle(update, _fix.Create<CancellationToken>()));
            mock.Verify(r => r.GetByIdAsync(user.Id), Times.Once());
        }


        [Test]
        public async Task Handle_InvalidPassword()
        {
            var currPassword = "Test1234!";
            var invalidPassword = "myInvalidPaasss!";
            var encryptedCurrPassword = _encryptor.Encrypt(currPassword);
            var newPasword = "newPeiasajd!";

            var user = _fix.Build<User>()
                .FromFactory(() => new User(
                    name: "idk",
                    email: "user@example.com",
                    passwordHash: encryptedCurrPassword,
                    address: _fix.Create<Address>()
                ))
                .Create();

            var update = new UpdateUserCommand(
                    Email: user.Email,
                    Address: user.Address,
                    CurrentPassword: invalidPassword,
                    NewPassword: newPasword);


            var mock = new Mock<IUserRepo>();
            mock.Setup(r => r.GetByIdAsync(user.Id))
                .ReturnsAsync(() => user);

            var contextMock = new Mock<IIdentityProvider>();
            contextMock.Setup(x => x.GetCurrentUserId())
                .Returns(user.Id);

            var sut = CreateSut(mock.Object, contextMock.Object);

            Assert.ThrowsAsync<UserWrongCredentialsException>(async () =>
                    await sut.Handle(update, _fix.Create<CancellationToken>()));
            mock.Verify(r => r.GetByIdAsync(user.Id), Times.Once());
        }
    }
}
