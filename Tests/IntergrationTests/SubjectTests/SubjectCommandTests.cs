using AutoFixture;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.CreateSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.RemoveSubject;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Domain.SeedWork;
using FusePortal.Infrastructure.Data;
using FusePortal.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntergrationTests.SubjectTests
{
    [TestFixture]
    public class SubjectCommandTests
    {
        private AppDbContext _context;
        private static readonly Fixture _fix = new();
        private static readonly User USER = _fix.Create<User>();

        [OneTimeSetUp]
        public async Task BeforeAllAsync()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _fix.Behaviors.Add(new OmitOnRecursionBehavior());

            foreach (var userUni in USER.Universities)
                USER.LeaveUniversity(userUni);
            var userRepo = new UserRepo(_context);
            await userRepo.AddAsync(USER);
        }

        [SetUp]
        public async Task BeforeEach()
        {
            _context.Subjects.RemoveRange(_context.Subjects.ToList());
            await _context.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task AfterAll()
        {
            await _context.DisposeAsync();
        }


        [Test]
        public async Task AddSubject_Success()
        {
            // Arrange
            var subject = _fix.Create<Subject>();
            var subjectRepo = new SubjectRepo(_context);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                        .Returns(USER.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new CreateSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);
            await _context.SaveChangesAsync();

            // Act
            await sut.Handle(new CreateSubjectCommand(
                        subject.Name,
                        subject.Metadata),
                    default);

            // Asset
            var res = await subjectRepo.GetByIdAsync(subject.Id, USER.Id);
            Assert.That(res, Is.Not.Null);
            Assert.That(res, Is.EqualTo(subject));
        }

        [Test]
        public async Task RemoveSubject_Success()
        {
            // Arrange
            var subject = _fix.Create<Subject>();
            var subjectRepo = new SubjectRepo(_context);
            await subjectRepo.AddAsync(subject);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                        .Returns(USER.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sut = new RemoveSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);
            await _context.SaveChangesAsync();

            // Act
            await sut.Handle(new RemoveSubjectCommand(subject.Id), default);

            // Asset
            var res = await subjectRepo.GetByIdAsync(subject.Id, USER.Id);
            Assert.That(res, Is.Null);
        }
    }
}
