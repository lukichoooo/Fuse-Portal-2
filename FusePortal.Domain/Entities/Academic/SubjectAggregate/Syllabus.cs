using System.ComponentModel.DataAnnotations;
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
        public Guid SubjectId { get; private set; }
        public Subject? Subject { get; private set; }

        public string? Metadata { get; private set; }

        internal Syllabus(
                string name,
                string content,
                Guid subjectId,
                string? metadata = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            SubjectId = subjectId;
            Metadata = metadata;
        }


        private Syllabus() { } // EF
    }
}

