using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Schedule : Entity
    {
        [Required]
        public DateTime Start { get; private set; }

        [Required]
        public DateTime End { get; private set; }

        [Required]
        public int SubjectId { get; private set; }
        public Subject? Subject { get; private set; }

        public string? Location { get; private set; }
        public string? Metadata { get; private set; }

        public Schedule(
                int subjectId,
                DateTime start,
                DateTime end,
                string? location = null,
                string? metadata = null)
        {
            Start = start;
            End = end;
            SubjectId = subjectId;
            Location = location;
            Metadata = metadata;
        }
    }
}
