using Facet;
using FusePortal.Domain.Common.ValueObjects;

namespace FusePortal.Application.Common
{
    [Facet(typeof(Address),
            Include =
            [
                nameof(Address.City),
                nameof(Address.Country),
            ]),
    ]
    public partial record AddressDto;
}
