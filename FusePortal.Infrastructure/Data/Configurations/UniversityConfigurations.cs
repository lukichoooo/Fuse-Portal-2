using FusePortal.Domain.Entities.UniversityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FusePortal.Infrastructure.Data.Configurations
{
    public class UniversityConfigurations : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.OwnsOne(u => u.Address, a =>
            {
                a.Property(p => p.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                a.Property(p => p.City)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }

}
