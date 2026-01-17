using FluentValidation;
using FusePortal.Application.Common.DtoValidators;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.Users.Commands.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(config.PasswordMinLength)
                .MaximumLength(config.PasswordMaxLength);

            RuleFor(x => x.Address)
                .SetValidator(new AddressDtoValidator());
        }
    }
}
