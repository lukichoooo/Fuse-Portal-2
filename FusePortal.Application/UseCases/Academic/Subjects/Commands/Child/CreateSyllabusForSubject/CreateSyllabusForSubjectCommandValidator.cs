using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateSyllabusForSubject
{
    public class CreateSyllabusForSubjectCommandValidator : AbstractValidator<CreateSyllabusForSubjectCommand>
    {
        public CreateSyllabusForSubjectCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(config.NameMaxLength);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(config.FileCharactersMax);
        }
    }
}
