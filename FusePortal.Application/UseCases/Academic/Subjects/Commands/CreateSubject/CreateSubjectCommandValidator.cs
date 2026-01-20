using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.CreateSubject
{
    public class CreateSubjectCommandValidator : AbstractValidator<CreateSubjectCommand>
    {
        public CreateSubjectCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(config.NameMaxLength);

            RuleFor(x => x.Metadata)
                .NotEmpty()
                .MaximumLength(config.MessageMaxLength);
        }
    }
}
