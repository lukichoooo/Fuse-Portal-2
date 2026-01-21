using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.Common.ValueObjects.LectureDate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.SubjectAggregate
{
    public class Schedule : Entity
    {
        [Required]
        public LectureDate LectureDate { get; private set; }

        [Required]
        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; private set; }

        public string? Location { get; private set; }
        public string? Metadata { get; private set; }

        public Schedule(
                Guid subjectId,
                LectureDate lectureDate,
                string? location = null,
                string? metadata = null)
        {
            LectureDate = lectureDate;
            SubjectId = subjectId;
            Location = location;
            Metadata = metadata;
        }

        private Schedule() { } // EF
    }
}
