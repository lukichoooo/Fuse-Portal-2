using FusePortal.Domain.Entities.Convo.ChatAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FusePortal.Infrastructure.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.CountNumber);

            builder.HasIndex(m => m.Id)
                .IsUnique();
            builder.Property(m => m.Id)
                .IsRequired();
        }
    }
}
