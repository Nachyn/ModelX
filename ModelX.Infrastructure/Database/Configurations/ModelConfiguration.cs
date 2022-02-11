using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelX.Domain.Entities;

namespace ModelX.Infrastructure.Database.Configurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.ToTable("Models");

        builder.HasKey(m => m.AttachmentId);
        builder.HasOne(m => m.Attachment)
            .WithOne(a => a.Model)
            .HasForeignKey<Model>(m => m.AttachmentId);
    }
}