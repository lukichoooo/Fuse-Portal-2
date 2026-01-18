using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Entities.Academic.SubjectAggregate.SubjectDomainEvents;
using FusePortal.Domain.Entities.Identity.UserAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public class Subject : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public Guid UserId { get; private set; }
        public User? User { get; private set; }

        public int? Grade { get; private set; }

        public string? Metadata { get; private set; }

        private readonly List<Schedule> _schedules = [];
        public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();

        private readonly List<Lecturer> _lecturers = [];
        public IReadOnlyCollection<Lecturer> Lecturers => _lecturers.AsReadOnly();

        private readonly List<Syllabus> _syllabuses = [];
        public IReadOnlyCollection<Syllabus> Syllabuses => _syllabuses.AsReadOnly();

        public Subject(
                string name,
                Guid userId,
                int? grade = null,
                string? metadata = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            UserId = userId;
            Grade = grade;
            Metadata = metadata;

            AddDomainEvent(new SubjectCreatedEvent(Id, UserId));
        }

        public void AddSchedule(Schedule schedule)
        {
            if (_schedules.Contains(schedule)) return;

            _schedules.Add(schedule);
            AddDomainEvent(new SubjectScheduleAddedEvent(Id, schedule));
        }

        public void AddLecturer(Lecturer lecturer)
        {
            if (_lecturers.Contains(lecturer)) return;

            _lecturers.Add(lecturer);
            AddDomainEvent(new SubjectLecturerAddedEvent(Id, lecturer));
        }

        public void AddSyllabus(Syllabus syllabus)
        {
            if (_syllabuses.Contains(syllabus)) return;

            _syllabuses.Add(syllabus);
            AddDomainEvent(new SubjectSyllabusAddedEvent(Id, syllabus));
        }


        private Subject() { } // EF
    }
}

