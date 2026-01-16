using Facet;
using FusePortal.Domain.Common.ValueObjects;

namespace FusePortal.Application.Common
{
    [Facet(typeof(Address),
            Include =
            [
                nameof(Address.City),
                nameof(Address.Country),
            ],
            GenerateToSource = true,
            NullableProperties = false
            ),
    ]
    public partial record AddressDto
    {
        public static implicit operator Address(AddressDto dto)
            => new(country: dto.Country,
                    city: dto.City);
    }
}
