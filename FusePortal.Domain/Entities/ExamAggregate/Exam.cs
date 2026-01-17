using System.ComponentModel.DataAnnotations;
using FusePortal.Domain.Entities.SubjectAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.ExamAggregate
{
    public class Exam : Entity, IAggregateRoot
    {
        [Required]
        public string Questions { get; private set; }

        public string Answers { get; private set; } = "";

        public string? Results { get; private set; }

        public int? ScoreFrom100 { get; private set; }

        [Required]
        public Guid SubjectId { get; private set; }
        public Subject? Subject { get; private set; }


        public Exam(
                string questions,
                string answers,
                Guid subjectid,
                string? results = null,
                int? scoreFrom100 = null)
        {
            Questions = questions ?? throw new ExamDomainException($"field cant be null or empty: {nameof(questions)}");
            Answers = answers ?? throw new ExamDomainException($"field cant be null or empty: {nameof(answers)}");
            SubjectId = subjectid;
            Results = results;
            ScoreFrom100 = scoreFrom100;
        }

        private Exam() { } // EF
    }
}
