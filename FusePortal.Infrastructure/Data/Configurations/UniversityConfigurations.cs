using FusePortal.Domain.Entities.Academic.UniversityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FusePortal.Infrastructure.Data.Configurations
{
    public class UniversityConfigurations : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder.OwnsOne(u => u.Address);
        }
    }

}
