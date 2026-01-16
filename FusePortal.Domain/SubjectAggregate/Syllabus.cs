using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Syllabus : Entity
    {
        [Required]
        public required string Name { get; set; } = null!;

        [Required]
        public required string Content { get; set; } = null!;

        [Required]
        public required int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public string? Metadata { get; set; }
    }
}

