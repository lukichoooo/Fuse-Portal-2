using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FusePortal.Domain.Entities.Academic.ExamAggregate.ExamDomainEvents;
using FusePortal.Domain.Entities.Academic.SubjectAggregate;
using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Entities.Academic.ExamAggregate
{
    public class Exam : Entity, IAggregateRoot
    {
        [Required]
        public string Questions { get; private set; }

        public string? Answers { get; private set; }

        public string? Results { get; private set; }

        public int? ScoreFrom100 { get; private set; }

        [Required]
        [ForeignKey(nameof(Subject))]
        public Guid SubjectId { get; private set; }


        public Exam(
                string questions,
                Guid subjectid,
                string? answers = null,
                string? results = null,
                int? scoreFrom100 = null)
        {
            Questions = questions ?? throw new ExamDomainException($"field cant be null or empty: {nameof(questions)}");
            Answers = answers;
            SubjectId = subjectid;
            Results = results;
            ScoreFrom100 = scoreFrom100;

            AddDomainEvent(new ExamCreatedEvent(Id));
        }


        public void FillAnswers(string answers)
        {
            Answers = answers ?? throw new ExamDomainException($"field {nameof(answers)} cant be null or empty");

            AddDomainEvent(new ExamAnswersFilledEvent(Id, answers));
        }

        public void GradeTheExam(string results, int? scoreFrom100)
        {
            if (Results != null)
                throw new ExamDomainException("this exam has already been graded");

            Results = results ?? throw new ExamDomainException($"field cant be null or empty: {nameof(results)}");
            ScoreFrom100 = scoreFrom100;

            AddDomainEvent(new ExamGradedEvent(Id, scoreFrom100));
        }

        public void UpdateExamGrade(string results, int? scoreFrom100)
        {
            if (Results == null)
            {
                GradeTheExam(results, scoreFrom100);
                return;
            }
            var oldGrade = ScoreFrom100;

            Results = results ?? throw new ExamDomainException($"field cant be null or empty: {nameof(results)}");
            ScoreFrom100 = scoreFrom100;

            AddDomainEvent(new ExamResultUpdatedEvent(Id, oldGrade, scoreFrom100));
        }


        private Exam() { } // EF
    }
}
