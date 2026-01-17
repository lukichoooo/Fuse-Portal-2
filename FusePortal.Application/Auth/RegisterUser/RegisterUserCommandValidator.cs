using FluentValidation;
using FusePortal.Application.Common.Settings;
using FusePortal.Application.Common.Validators;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.Auth.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;


            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(config.PasswordMinLength)
                .MaximumLength(config.PasswordMaxLength);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(config.NameMaxLength);

            RuleFor(x => x.Address)
                .SetValidator(new AddressValidator());
        }
    }
}
