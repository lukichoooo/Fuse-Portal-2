using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public class Syllabus : Entity
    {
        [Required]
        public string Name { get; private set; }

        [Required]
        public string Content { get; private set; }

        [Required]
        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; private set; }

        internal Syllabus(
                string name,
                string content,
                Guid subjectId)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            SubjectId = subjectId;
        }


        private Syllabus() { } // EF
    }
}

