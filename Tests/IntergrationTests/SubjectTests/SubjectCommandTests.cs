using AutoFixture;
using FusePortal.Application.Interfaces.Auth;
using FusePortal.Application.Interfaces.EventDispatcher;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateLecturerForSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateScheduleForSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusForSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveLecturerFromSubjectCommand;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveScheduleFromSubjectCommand;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveSyllabusFromSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.CreateSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Commands.RemoveSubject;
using FusePortal.Application.UseCases.Academic.Subjects.Exceptions;
using FusePortal.Domain.Common.ValueObjects.LectureDate;
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
            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                        .Returns(USER.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var subjectRepo = new SubjectRepo(_context);
            var sut = new CreateSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);

            await _context.SaveChangesAsync();

            // Act
            var subjectName = _fix.Create<string>();
            var subjectMetaData = _fix.Create<string>();
            var id = await sut.Handle(new CreateSubjectCommand(
                        subjectName,
                        subjectMetaData),
                    default);

            // Asset
            var res = await subjectRepo.GetByIdAsync(id, USER.Id);
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Id, Is.EqualTo(id));
        }


        [Test]
        public async Task RemoveSubject_Success()
        {
            // Arrange
            var subject = new Subject(_fix.Create<string>(), USER.Id);
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


        [Test]
        public async Task RemoveSubject_WrongUser()
        {
            // Arrange
            var subject = new Subject(_fix.Create<string>(), USER.Id);
            var subjectRepo = new SubjectRepo(_context);
            await subjectRepo.AddAsync(subject);

            var wrongUserId = Guid.NewGuid();
            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                        .Returns(wrongUserId);

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
            // Asset
            Assert.ThrowsAsync<SubjectNotFoundException>(async () =>
                await sut.Handle(new RemoveSubjectCommand(subject.Id), default));
        }



        [Test]
        public async Task CreatePropsForSubject_Success()
        {
            // Arrange
            var subject = new Subject(_fix.Create<string>(), USER.Id);
            var subjectRepo = new SubjectRepo(_context);

            foreach (var l in subject.Lecturers)
                subject.RemoveLecturer(l.Id);
            foreach (var s in subject.Schedules)
                subject.RemoveLecturer(s.Id);
            foreach (var s in subject.Syllabuses)
                subject.RemoveLecturer(s.Id);

            await subjectRepo.AddAsync(subject);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                        .Returns(USER.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sutSyllabus = new CreateSyllabusForSubjectCommandHandler(
                    identityMock.Object,
                    subjectRepo,
                    uow);

            var sutSchedule = new CreateScheduleForSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);

            var sutLecturer = new CreateLecturerForSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);

            await _context.SaveChangesAsync();

            var syllabus = _fix.Create<Syllabus>();
            var lecturer = _fix.Create<Lecturer>();
            var schedule = _fix.Create<Schedule>();

            // Act
            await sutSyllabus.Handle(new CreateSyllabusForSubjectCommand(
                        syllabus.Name,
                        syllabus.Content,
                        subject.Id),
                    default);

            await sutSchedule.Handle(new CreateScheduleForSubjectCommand(
                        subject.Id,
                        schedule.LectureDate,
                        schedule.Location,
                        schedule.Metadata),
                    default);

            await sutLecturer.Handle(new CreateLecturerForSubjectCommand(
                        subject.Id,
                        lecturer.Name),
                    default);

            // Asset
            var res = await subjectRepo.GetByIdAsync(subject.Id, USER.Id);
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Lecturers.First().Name, Is.EqualTo(lecturer.Name));
            Assert.That(res.Schedules.First().LectureDate, Is.EqualTo(schedule.LectureDate));
            Assert.That(res.Syllabuses.First().Name, Is.EqualTo(syllabus.Name));
        }



        [Test]
        public async Task RemovePropsFromSubject_Success()
        {
            // Arrange
            var subject = new Subject(_fix.Create<string>(), USER.Id);
            var subjectRepo = new SubjectRepo(_context);

            foreach (var l in subject.Lecturers)
                subject.RemoveLecturer(l.Id);
            foreach (var s in subject.Schedules)
                subject.RemoveLecturer(s.Id);
            foreach (var s in subject.Syllabuses)
                subject.RemoveLecturer(s.Id);

            await subjectRepo.AddAsync(subject);

            var identityMock = new Mock<IIdentityProvider>();
            identityMock.Setup(c => c.GetCurrentUserId())
                        .Returns(USER.Id);

            var dispatcher = new Mock<IDomainEventDispatcher>();
            dispatcher.Setup(d => d.DispatchAsync(
                       It.IsAny<IEnumerable<IDomainEvent>>(),
                       It.IsAny<CancellationToken>()));

            var uow = new EfUnitOfWork(_context, dispatcher.Object);

            var sutSyllabus = new RemoveSyllabusFromSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);

            var sutSchedule = new RemoveScheduleFromSubjectCommandHandler(
                    subjectRepo,
                    identityMock.Object,
                    uow);

            var sutLecturer = new RemoveLecturerFromSubjectCommandHandler(
                    identityMock.Object,
                    subjectRepo,
                    uow);


            var syllabus = new Syllabus(_fix.Create<string>(), _fix.Create<string>(), subject.Id);
            subject.AddSyllabus(syllabus.Name, syllabus.Content);

            var lecturer = new Lecturer(_fix.Create<string>(), subject.Id);
            subject.AddLecturer(lecturer.Name);

            var schedule = new Schedule(subject.Id, _fix.Create<LectureDate>(), _fix.Create<string>(), _fix.Create<string>());
            subject.AddSchedule(schedule.LectureDate, schedule.Location, schedule.Metadata);

            await _context.SaveChangesAsync();

            // Act
            await sutSyllabus.Handle(new RemoveSyllabusFromSubjectCommand(
                        subject.Id,
                        subject.Syllabuses.First().Id),
                    default);

            await sutSchedule.Handle(new RemoveScheduleFromSubjectCommand(
                        subject.Id,
                        subject.Schedules.First().Id),
                    default);

            await sutLecturer.Handle(new RemoveLecturerFromSubjectCommand(
                        subject.Id,
                        subject.Lecturers.First().Id),
                    default);

            // Asset
            var res = await subjectRepo.GetByIdAsync(subject.Id, USER.Id);
            Assert.That(res, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(res.Lecturers, Is.Empty);
                Assert.That(res.Schedules, Is.Empty);
                Assert.That(res.Syllabuses, Is.Empty);
            }

        }
    }
}
