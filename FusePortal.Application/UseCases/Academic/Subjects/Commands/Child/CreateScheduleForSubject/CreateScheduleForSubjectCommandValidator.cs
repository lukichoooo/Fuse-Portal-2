using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateScheduleForSubject
{
    public class CreateScheduleForSubjectCommandValidator : AbstractValidator<CreateScheduleForSubjectCommand>
    {
        public CreateScheduleForSubjectCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.SubjectId)
                .NotEmpty();

            RuleFor(x => x.Location)
                .NotEmpty()
                .MaximumLength(config.FileCharactersMax);

            RuleFor(x => x.Metadata)
                .NotEmpty()
                .MaximumLength(config.FileCharactersMax);
        }
    }
}
