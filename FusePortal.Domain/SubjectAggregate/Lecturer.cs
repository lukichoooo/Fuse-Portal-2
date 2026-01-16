using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.SubjectAggregate
{
    public class Lecturer : Entity
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public int SubjectId { get; private set; }
        public Subject? Subject { get; private set; }

        public Lecturer(string name, int subjectId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SubjectId = subjectId;
        }
    }
}
