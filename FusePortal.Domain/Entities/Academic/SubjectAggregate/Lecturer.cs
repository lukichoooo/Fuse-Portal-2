using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public class Lecturer : Entity
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public Guid SubjectId { get; private set; }
        public Subject? Subject { get; private set; }

        public Lecturer(string name, Guid subjectId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SubjectId = subjectId;
        }

        private Lecturer() { } // EF
    }
}
