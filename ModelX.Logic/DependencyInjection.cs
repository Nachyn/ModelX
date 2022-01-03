using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelX.Logic.Common.AppConfigs.Configurations;
using ModelX.Logic.Common.Behaviours;
using ModelX.Logic.Common.Extensions;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddLogic(this IServiceCollection services,
        IConfiguration configuration,
        string webHostEnvironmentContentRootPath)
    {
        services.AddAppSettingHelpers(configuration, webHostEnvironmentContentRootPath);

        var mapperConfiguration =
            new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
        mapperConfiguration.AssertConfigurationIsValid();
        services.AddSingleton(mapperConfiguration.CreateMapper());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddPropertyValidatorsFromAssembly();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}