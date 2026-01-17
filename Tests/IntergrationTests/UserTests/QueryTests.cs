using AutoFixture;
using FusePortal.Application.Unis.Queries.GetUnisPage;
using FusePortal.Application.Users.Queries.GetUsersPage;
using FusePortal.Domain.Entities.UniversityAggregate;
using FusePortal.Domain.Entities.UserAggregate;
using FusePortal.Infrastructure.Data;
using FusePortal.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;

namespace IntergrationTests.UserTests
{
    [TestFixture]
    public class QueryTests
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



        [TestCase(null, 3, 3)]
        [TestCase(null, 5, 12)]
        [TestCase(null, 2, 7)]
        [TestCase(null, 1, 4)]
        public async Task GetUsersPageQuery_Success(Guid? lastId, int pageSize, int n)
        {
            IUserRepo repo = new UserRepo(_context);
            var users = Enumerable.Range(1, n)
                    .Select(_ => _fix.Create<User>())
                    .ToList();
            await _context.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            var sut = new GetUsersPageQueryHandler(repo);

            HashSet<Guid> seenId = [];
            for (int i = 0; i < n; i += pageSize)
            {
                var res = await sut.Handle(
                        new GetUsersPageQuery(lastId, pageSize),
                        default);

                Assert.That(res, Is.Not.Null);
                foreach (var u in res)
                {
                    Assert.That(seenId, Does.Not.Contain(u.Id));
                    seenId.Add(u.Id);
                }
                lastId = res.Last().Id;
            }
            Assert.That(seenId, Has.Count.EqualTo(n));
        }


        [TestCase(null, 3, 3)]
        [TestCase(null, 5, 12)]
        [TestCase(null, 2, 7)]
        [TestCase(null, 1, 4)]
        public async Task GetUnisPageQuery_Success(Guid? lastId, int pageSize, int n)
        {
            IUniRepo repo = new UniRepo(_context);
            var unis = Enumerable.Range(1, n)
                    .Select(_ => _fix.Create<University>())
                    .ToList();
            await _context.AddRangeAsync(unis);
            await _context.SaveChangesAsync();

            var sut = new GetUnisPageQueryHandler(repo);

            HashSet<Guid> seenId = [];
            for (int i = 0; i < n; i += pageSize)
            {
                var res = await sut.Handle(
                        new GetUnisPageQuery(lastId, pageSize),
                        default);

                Assert.That(res, Is.Not.Null);
                foreach (var u in res)
                {
                    Assert.That(seenId, Does.Not.Contain(u.Id));
                    seenId.Add(u.Id);
                }
                lastId = res.Last().Id;
            }
            Assert.That(seenId, Has.Count.EqualTo(n));
        }


    }
}
