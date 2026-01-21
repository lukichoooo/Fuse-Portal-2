using FluentValidation;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.SavePortal
{
    public class SavePortalCommandValidator : AbstractValidator<SavePortalCommand>
    {
        public SavePortalCommandValidator()
        {
            RuleFor(x => x.PortalPageText)
                .NotEmpty()
                .MaximumLength(20000);
        }
    }
}
