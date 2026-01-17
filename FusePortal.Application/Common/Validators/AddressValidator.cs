using FluentValidation;
using FusePortal.Domain.Common.ValueObjects.Address;

namespace FusePortal.Application.Common.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();
        }
    }
}
