using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Exams.Commands.GradeExam
{
    public class GradeExamCommandValidator : AbstractValidator<GradeExamCommand>
    {
        public GradeExamCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x.ExamId)
                .NotEmpty();

            RuleFor(x => x.Answers)
                .NotEmpty()
                .MaximumLength(config.FileCharactersMax);
        }
    }
}
