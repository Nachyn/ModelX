using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelX.Domain.Entities;

namespace ModelX.Infrastructure.Database.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");

        builder.Property(f => f.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(f => f.Path)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasAlternateKey(f => f.Path);
    }
}