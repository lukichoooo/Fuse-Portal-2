using Facet;
using FusePortal.Domain.Entities.Content.FileEntityAggregate;

namespace FusePortal.Application.UseCases.Content.Files
{
    [Facet(typeof(FileEntity),
            Include = [
                nameof(FileEntity.Name),
                nameof(FileEntity.Text)
            ])]
    public partial record FileDto;
}
