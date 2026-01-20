using Facet;
using FusePortal.Domain.Entities.Academic.ExamAggregate;

namespace FusePortal.Application.UseCases.Academic.Exams
{
    [Facet(typeof(Exam),
        Include =
        [
            nameof(Exam.Id),
            nameof(Exam.Questions),
            nameof(Exam.ScoreFrom100),
            nameof(Exam.Answers),
            nameof(Exam.Results),
            nameof(Exam.SubjectId),
        ])
    ]
    public partial record ExamDto;

}
