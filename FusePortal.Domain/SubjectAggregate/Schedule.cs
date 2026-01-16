using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Schedule : Entity
    {
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }

        [Required]
        public required int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public string? Location { get; set; }
        public string? Metadata { get; set; }
    }
}
