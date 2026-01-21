using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.Child.RemoveScheduleFromSubjectCommand
{
    public class RemoveScheduleFromSubjectCommandValidator : AbstractValidator<RemoveScheduleFromSubjectCommand>
    {
        public RemoveScheduleFromSubjectCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x.SubjectId)
                .NotEmpty();

            RuleFor(x => x.ScheduleId)
                .NotEmpty();
        }
    }
}
