using FusePortal.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FusePortal.Infrastructure.Data.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // TODO inject configurations from file
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(256); // idk

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
