using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModelX.Domain.Enums;
using ModelX.Domain.Helpers;

namespace ModelX.Infrastructure.Database.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
    {
        var enumValues = Enum.GetValues(typeof(Roles)).Cast<Roles>();

        var roles = enumValues
            .Select(@enum =>
            {
                var id = Convert.ToInt32(@enum);
                var enumDescription = @enum.GetEnumDescription();
                return new IdentityRole<int>
                {
                    Id = id,
                    Name = enumDescription,
                    NormalizedName = enumDescription.ToUpper()
                };
            })
            .ToArray();

        builder.HasData(roles);
    }
}