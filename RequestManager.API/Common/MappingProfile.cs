using AutoMapper;
using System.Reflection;

namespace RequestManager.API.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);

        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(x => HasInterface(x, mapFromType)) && !t.IsAbstract).ToList();

       
        var argumentTypes = new Type[] { typeof(Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(x => HasInterface(x, mapFromType)).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }

    private static bool HasInterface(Type t, Type mapFromType) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
}