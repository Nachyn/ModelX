using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelX.Logic.Common.AppConfigs.Entities;
using ModelX.Logic.Common.AppConfigs.Main;

namespace ModelX.Logic.Common.AppConfigs.Configurations;

public static class AppSettingHelperExtensions
{
    public static IServiceCollection AddAppSettingHelpers(
        this IServiceCollection services
        , IConfiguration configuration
        , string webHostEnvironmentContentRootPath)
    {
        services.AddOptions();

        services.Configure<PasswordIdentitySettings>(
            configuration.GetSection("PasswordIdentitySettings"));

        services.Configure<AuthOptions>(
            configuration.GetSection("AuthOptions"));

        services.Configure<RootFileFolderDirectory>(
            directory =>
            {
                directory.RootFileFolder =
                    Path.Combine(webHostEnvironmentContentRootPath, "UserFiles");
            }
        );

        ConfigureEntities(services, configuration);

        return services;
    }

    private static void ConfigureEntities(
        IServiceCollection services
        , IConfiguration configuration)
    {
        const string entitySection = "EntitySettings:";

        services.Configure<AppUserSettings>(
            configuration.GetSection($"{entitySection}AppUser"));
    }
}