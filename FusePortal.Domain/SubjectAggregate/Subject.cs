using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Subject : Entity, IAggregateRoot
    {
        [Required]
        public required string Name { get; set; } = null!;

        [Required]
        public required int UserId { get; set; }
        public User? User { get; set; }

        public int? Grade { get; set; }

        public string? Metadata { get; set; }

        public List<Schedule> Schedules { get; set; } = [];
        public List<Lecturer> Lecturers { get; set; } = [];
        public List<Syllabus> Syllabuses { get; set; } = [];
    }
}

