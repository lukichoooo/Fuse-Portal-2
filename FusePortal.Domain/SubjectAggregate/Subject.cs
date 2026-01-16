using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Subject : Entity, IAggregateRoot
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public int UserId { get; private set; }
        public User? User { get; private set; }

        public int? Grade { get; private set; }

        public string? Metadata { get; private set; }

        private readonly List<Schedule> _schedules;
        public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();

        private readonly List<Lecturer> _lecturers;
        public IReadOnlyCollection<Lecturer> Lecturers => _lecturers.AsReadOnly();

        private readonly List<Syllabus> _syllabuses;
        public IReadOnlyCollection<Syllabus> Syllabuses => _syllabuses.AsReadOnly();
    }
}

