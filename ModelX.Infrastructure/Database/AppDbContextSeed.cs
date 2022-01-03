using Microsoft.EntityFrameworkCore;

namespace ModelX.Infrastructure.Database;

public static class AppDbContextSeed
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        {
            await context.Database.MigrateAsync();
        }
    }
}