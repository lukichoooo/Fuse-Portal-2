using FluentValidation;
using FusePortal.Application.Common.Settings;
using Microsoft.Extensions.Options;

namespace FusePortal.Application.Unis.Commands.Create
{
    public class CreateUniCommandValidator : AbstractValidator<CreateUniCommand>
    {
        public CreateUniCommandValidator(IOptions<ValidatorSettings> options)
        {
            var config = options.Value;

            RuleFor(p => p.Name)
                .NotEmpty()
                .MinimumLength(config.UniNameMinLength)
                .MaximumLength(config.UniNameMaxLength);
        }
    }
}
