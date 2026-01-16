using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;
using FusePortal.Domain.SubjectAggregate;

namespace FusePortal.Domain.ExamAggregate
{
    public class Exam : Entity, IAggregateRoot
    {
        [Required]
        public string Questions { get; private set; }

        public string Answers { get; private set; } = "";

        public string? Results { get; private set; }

        public int? ScoreFrom100 { get; private set; }

        [Required]
        public int SubjectId { get; private set; }
        public Subject? Subject { get; private set; }
    }
}
