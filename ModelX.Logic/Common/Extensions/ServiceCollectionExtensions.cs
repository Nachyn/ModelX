using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ModelX.Logic.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPropertyValidatorsFromAssembly(
        this IServiceCollection services)
    {
        var validators = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(t =>
                t.BaseType?.ToString() ==
                "FluentValidation.Validators.PropertyValidator`2[T,System.String]")
            .ToList();

        validators.ForEach(v => { services.AddScoped(v); });

        return services;
    }
}