using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Lecturer : Entity
    {
        public required string Name { get; set; }

        [Required]
        public required int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}
