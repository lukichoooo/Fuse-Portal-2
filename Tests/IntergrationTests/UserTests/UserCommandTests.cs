using AutoFixture;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Application.UseCases.Identity.Users.Commands.AddUniToUser;
using FusePortal.Application.UseCases.Identity.Users.Commands.RemoveUniFromUser;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Domain.SeedWork;
using FusePortal.Infrastructure.Data;
using FusePortal.Infrastructure.Repo;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntergrationTests.UserTests
{
    [TestFixture]
    public class UserCommandTests
    {
        private AppDbContext _context;
        private static readonly Fixture _fix = new();

        [OneTimeSetUp]
        public void BeforeAll()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _fix.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [SetUp]
        public async Task BeforeEach()
        {
            _context.Universities.RemoveRange(_context.Universities.ToList());
            _context.Universities.RemoveRange(_context.Universities.ToList());
            _context.Users.RemoveRange(_context.Users.ToList());
            await _context.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task AfterAll()
        {
            await _context.DisposeAsync();
        }


        [Test]
        public async Task AddUniToUser_Success()
        {
            // Arrange
            var user = _fix.Create<User>();
            foreach (var userUni in user.Universities)
                user.RemoveUniversity(userUni);
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(user);

            var uni = _fix.Create<University>();
            var uniRepo = new UniRepo(_context);
            await uniRepo.AddAsync(uni);



            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new AddUniToUserCommandHandler(
                    userRepo,
                    uniRepo,
                    identityMock.Object,
                    uow);
            await _context.SaveChangesAsync();

            // Act
            await sut.Handle(new AddUniToUserCommand(uni.Id), default);

            // Asset
            Assert.That(user.Universities, Has.Count.EqualTo(1));
            Assert.That(user.Universities.First().Id, Is.EqualTo(uni.Id));
            Assert.That(_context.Users.First()
                    .Universities.First().Id,
                    Is.EqualTo(uni.Id));
        }

        [Test]
        public async Task AddUniToUser_AlreadyAdded()
        {
            // Arrange
            var user = _fix.Create<User>();
            foreach (var userUni in user.Universities)
                user.RemoveUniversity(userUni);
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(user);

            var uni = _fix.Create<University>();
            var uniRepo = new UniRepo(_context);
            await uniRepo.AddAsync(uni);


            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new AddUniToUserCommandHandler(
                    userRepo,
                    uniRepo,
                    identityMock.Object,
                    uow);
            user.AddUniversity(uni);

            await _context.SaveChangesAsync();

            // Act
            await sut.Handle(new AddUniToUserCommand(uni.Id), default);

            // Asset
            Assert.That(user.Universities, Has.Count.EqualTo(1));
            Assert.That(user.Universities.First().Id, Is.EqualTo(uni.Id));
            Assert.That(_context.Users.First()
                    .Universities.First().Id,
                    Is.EqualTo(uni.Id));
        }


        [Test]
        public async Task RemoveUniFromUser_Success()
        {
            // Arrange
            var user = _fix.Create<User>();
            foreach (var userUni in user.Universities)
                user.RemoveUniversity(userUni);
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(user);

            var uni = _fix.Create<University>();
            var uniRepo = new UniRepo(_context);
            await uniRepo.AddAsync(uni);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new RemoveUniFromUserCommandHandler(
                    userRepo,
                    uniRepo,
                    identityMock.Object,
                    uow);
            user.AddUniversity(uni);

            await _context.SaveChangesAsync();

            // Act
            await sut.Handle(new RemoveUniFromUserCommand(uni.Id), default);

            // Asset
            Assert.That(user.Universities, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task RemoveUniFromUser_Empty()
        {
            // Arrange
            var user = _fix.Create<User>();
            foreach (var userUni in user.Universities)
                user.RemoveUniversity(userUni);
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(user);

            var uni = _fix.Create<University>();
            var uniRepo = new UniRepo(_context);
            await uniRepo.AddAsync(uni);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new RemoveUniFromUserCommandHandler(
                    userRepo,
                    uniRepo,
                    identityMock.Object,
                    uow);

            await _context.SaveChangesAsync();

            // Act
            await sut.Handle(new RemoveUniFromUserCommand(uni.Id), default);

            // Asset
            Assert.That(user.Universities, Has.Count.EqualTo(0));
        }

    }
}
