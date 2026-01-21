using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveLecturerFromSubjectCommand
{
    public class RemoveLecturerFromSubjectCommandValidator : AbstractValidator<RemoveLecturerFromSubjectCommand>
    {
        public RemoveLecturerFromSubjectCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.SubjectId)
                .NotEmpty();

            RuleFor(x => x.LecturerId)
                .NotEmpty();
        }
    }
}
