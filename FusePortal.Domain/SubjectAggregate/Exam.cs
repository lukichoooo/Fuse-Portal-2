using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Exam : Entity
    {
        [Required]
        public required string Questions { get; set; } = null!;

        public string Answers { get; set; } = "";

        public string? Results { get; set; }

        public int? ScoreFrom100 { get; set; }

        [Required]
        public required int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}
