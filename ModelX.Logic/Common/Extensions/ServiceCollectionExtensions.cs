using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModelX.Logic.Common.Validators;

namespace ModelX.Logic.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPropertyValidatorsFromAssembly(
        this IServiceCollection services)
    {
        var validators = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(t =>
                t.Namespace == "ModelX.Logic.Common.Validators" &&
                t.Name != nameof(CommonValidatorsResource))
            .ToList();

        validators.ForEach(v => { services.AddScoped(v); });

        return services;
    }
}