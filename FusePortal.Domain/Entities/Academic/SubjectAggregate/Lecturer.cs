using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public class Lecturer : Entity
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; private set; }

        public Lecturer(string name, Guid subjectId)
        {
            Name = name ?? throw new SubjectDomainException($"{nameof(name)} cannot be null");
            SubjectId = subjectId;
        }

        private Lecturer() { } // EF
    }
}
