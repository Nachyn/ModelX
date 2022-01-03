using System.Reflection;
using AutoMapper;

namespace ModelX.Logic.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodName = nameof(IMapFrom<int>.Mapping);
            var interfaceName = typeof(IMapFrom<>).Name;
            var methodInfo = type.GetMethod(methodName)
                             ?? type.GetInterface(interfaceName)
                                 .GetMethod(methodName);

            methodInfo?.Invoke(instance, new object[] {this});
        }
    }
}