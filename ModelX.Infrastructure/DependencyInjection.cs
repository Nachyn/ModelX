using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelX.Infrastructure.Database;
using ModelX.Infrastructure.Services.DateTimeService;
using ModelX.Infrastructure.Services.FileService;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.ExternalServices.DateTimeService;
using ModelX.Logic.Common.ExternalServices.FileService;

namespace ModelX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => { b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName); }));

        services.AddScoped<IAppDbContext>(provider =>
            provider.GetService<AppDbContext>());

        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddSingleton<IFileService, FileService>();

        return services;
    }
}