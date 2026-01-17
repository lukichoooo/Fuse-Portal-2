using Facet;
using FusePortal.Domain.Entities.UniversityAggregate;

namespace FusePortal.Application.Unis
{
    [Facet(typeof(University),
            Include =
            [
                nameof(University.Id),
                nameof(University.Name),
                nameof(University.Address),
            ]
            )]
    public partial record UniDto;
}
