using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.Common.ValueObjects.LectureDate;
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


        public void AddSchedule(LectureDate lectureDate, string? location, string? metadata)
        {
            var schedule = new Schedule(Id, lectureDate, location, metadata);

            if (_schedules.Contains(schedule)) return;
            _schedules.Add(schedule);
        }

        public void RemoveSchedule(Guid scheduleId)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId)
                ?? throw new SubjectDomainException($"schedule with Id={scheduleId} not found");
            _schedules.Remove(schedule);
        }

        public void AddLecturer(string name)
        {
            var lecturer = new Lecturer(name, Id);
            if (_lecturers.Contains(lecturer)) return;

            _lecturers.Add(lecturer);
        }

        public void RemoveLecturer(Guid lecturerId)
        {
            var lecturer = _lecturers.FirstOrDefault(l => l.Id == lecturerId)
                ?? throw new SubjectDomainException($"schedule with Id={lecturerId} not found");

            _lecturers.Remove(lecturer);
        }

        public void AddSyllabus(string name, string content)
        {
            var syllabus = new Syllabus(name, content, Id);
            if (_syllabuses.Contains(syllabus)) return;

            _syllabuses.Add(syllabus);
        }

        public void RemoveSyllabus(Guid syllabusId)
        {
            var syllabus = _syllabuses.FirstOrDefault(s => s.Id == syllabusId)
                ?? throw new SubjectDomainException($"schedule with Id={syllabusId} not found");

            _syllabuses.Remove(syllabus);
        }


        private Subject() { } // EF
    }
}

