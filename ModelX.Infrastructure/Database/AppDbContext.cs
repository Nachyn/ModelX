using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.ExternalServices.Database;

namespace ModelX.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<Token> Tokens { get; set; }

    public DbSet<Model> Models { get; set; }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    ///     Trim for string properties
    /// </summary>
    private void OnBeforeSaving()
    {
        var entries = ChangeTracker?.Entries();

        if (entries == null)
        {
            return;
        }

        foreach (var entry in entries)
        {
            var propertyValues =
                entry.CurrentValues.Properties.Where(p =>
                    p.ClrType == typeof(string));

            foreach (var property in propertyValues)
            {
                if (entry.CurrentValues[property.Name] != null)
                {
                    entry.CurrentValues[property.Name] =
                        entry.CurrentValues[property.Name].ToString().Trim();
                }
            }
        }
    }
}