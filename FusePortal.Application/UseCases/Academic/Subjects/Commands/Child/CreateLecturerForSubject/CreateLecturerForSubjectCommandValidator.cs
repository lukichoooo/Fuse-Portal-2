using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.CreateLecturerForSubject
{
    public class CreateLecturerForSubjectCommandValidator : AbstractValidator<CreateLecturerForSubjectCommand>
    {
        public CreateLecturerForSubjectCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x.SubjectId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(config.NameMaxLength);
        }
    }
}
