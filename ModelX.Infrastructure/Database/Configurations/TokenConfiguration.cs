using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelX.Domain.Entities;

namespace ModelX.Infrastructure.Database.Configurations;

public class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable("Tokens");

        builder.Property(t => t.Client).IsRequired();
        builder.Property(t => t.Value).IsRequired();
    }
}