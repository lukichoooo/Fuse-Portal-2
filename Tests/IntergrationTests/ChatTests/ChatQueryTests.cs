using AutoFixture;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.UseCases.Convo.Chats.Queries.GetChatsPage;
using FusePortal.Application.UseCases.Convo.Chats.Queries.GetMessagesPage;
using FusePortal.Domain.Entities.Convo.ChatAggregate;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Infrastructure.Data;
using FusePortal.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntergrationTests.ChatTests
{
    [TestFixture]
    public class ChatQueryTests
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
        public async Task GetChatsPage_Success(Guid? lastId, int pageSize, int n)
        {
            var userRepo = new UserRepo(_context);
            var user = _fix.Create<User>();
            await userRepo.AddAsync(user);

            var repo = new ChatRepo(_context);
            var chats = Enumerable.Range(1, n)
                    .Select(_ => new Chat(_fix.Create<string>(), user.Id))
                    .ToList();
            await _context.AddRangeAsync(chats);
            await _context.SaveChangesAsync();
            var identity = new Mock<IIdentityProvider>();
            identity.Setup(c => c.GetCurrentUserId())
                .Returns(user.Id);

            var sut = new GetChatsPageQueryHandler(repo, identity.Object);

            HashSet<Guid> seenId = [];
            for (int i = 0; i < n; i += pageSize)
            {
                var res = await sut.Handle(
                        new GetChatsPageQuery(lastId, pageSize),
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

        [TestCase(null, 1, 1)]
        public async Task GetChatsPage_WrongUserRequesting(Guid? lastId, int pageSize, int n)
        {
            var userRepo = new UserRepo(_context);
            var user = _fix.Create<User>();
            await userRepo.AddAsync(user);
            var anotherUserId = _fix.Create<Guid>();

            var repo = new ChatRepo(_context);
            var chats = Enumerable.Range(1, n)
                    .Select(_ => new Chat(_fix.Create<string>(), user.Id))
                    .ToList();
            await _context.AddRangeAsync(chats);
            await _context.SaveChangesAsync();
            var identity = new Mock<IIdentityProvider>();
            identity.Setup(c => c.GetCurrentUserId())
                .Returns(anotherUserId);

            var sut = new GetChatsPageQueryHandler(repo, identity.Object);

            var res = await sut.Handle(
                       new GetChatsPageQuery(lastId, pageSize),
                       default);

            Assert.That(res, Is.Empty);
        }

        [TestCase(null, 3, 3)]
        [TestCase(null, 5, 12)]
        [TestCase(null, 2, 7)]
        [TestCase(null, 1, 4)]
        public async Task GetChatWithMessagesPage_Success(int? topCountNumber, int pageSize, int n)
        {
            var userRepo = new UserRepo(_context);
            var user = _fix.Create<User>();
            await userRepo.AddAsync(user);

            var repo = new ChatRepo(_context);
            var chat = new Chat(_fix.Create<string>(), user.Id);
            await _context.AddAsync(chat);


            var messages = Enumerable.Range(1, n)
                    .Select(idx =>
                            {

                                var m = new Message(
                                             _fix.Create<string>(),
                                             idx % 2 != 0
                                             , chat.Id);

                                typeof(Message)
                                      .GetProperty(nameof(Message.CountNumber))!
                                      .SetValue(m, idx);

                                return m;
                            }
                    )
                    .ToList();

            foreach (var m in messages)
            {
                if (m.FromUser)
                    chat.SendMessage(m);
                else
                    chat.RecieveResponse(m);
            }

            var identity = new Mock<IIdentityProvider>();
            identity.Setup(c => c.GetCurrentUserId())
                    .Returns(user.Id);

            await _context.SaveChangesAsync();

            var sut = new GetMessagesPageQueryHandler(repo, identity.Object);

            HashSet<Guid> seenId = [];
            for (int i = 0; i < n; i += pageSize)
            {
                var res = await sut.Handle(
                        new GetMessagesPageQuery(chat.Id, topCountNumber, pageSize),
                        default);

                Assert.That(res, Is.Not.Null);
                foreach (var m in res)
                {
                    Assert.That(seenId, Does.Not.Contain(m.Id));
                    seenId.Add(m.Id);
                }
                topCountNumber = res.First().CountNumber;
            }
            Assert.That(seenId, Has.Count.EqualTo(n));
        }

    }
}
