using Facet;
using FusePortal.Domain.Entities.Academic.UniversityAggregate;

namespace FusePortal.Application.UseCases.Academic.Unis
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
