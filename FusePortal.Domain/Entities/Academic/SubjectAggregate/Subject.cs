using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey(nameof(User))]
        public Guid UserId { get; private set; }

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
                string? metadata = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            UserId = userId;
            Metadata = metadata;

            AddDomainEvent(new SubjectCreatedEvent(Id, UserId));
        }


        public void AddSchedule(Schedule schedule)
        {
            if (_schedules.Contains(schedule)) return;

            _schedules.Add(schedule);
        }

        public void RemoveSchedule(Schedule schedule)
        {
            if (!_schedules.Contains(schedule))
                throw new SubjectDomainException($"schedule with Id={schedule.Id} not found");

            _schedules.Remove(schedule);
        }

        public void AddLecturer(Lecturer lecturer)
        {
            if (_lecturers.Contains(lecturer)) return;

            _lecturers.Add(lecturer);
        }

        public void RemoveLecturer(Lecturer lecturer)
        {
            if (!_lecturers.Contains(lecturer))
                throw new SubjectDomainException($"schedule with Id={lecturer.Id} not found");


            _lecturers.Remove(lecturer);
        }

        public void AddSyllabus(Syllabus syllabus)
        {
            if (_syllabuses.Contains(syllabus)) return;

            _syllabuses.Add(syllabus);
        }

        public void RemoveSyllabus(Syllabus syllabus)
        {
            if (!_syllabuses.Contains(syllabus))
                throw new SubjectDomainException($"schedule with Id={syllabus.Id} not found");

            _syllabuses.Remove(syllabus);
        }


        private Subject() { } // EF
    }
}

