using FluentValidation;

namespace FusePortal.Application.Common.DtoValidators
{
    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();
        }
    }
}
