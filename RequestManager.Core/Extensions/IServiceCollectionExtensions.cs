using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;

namespace RequestManager.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddClassesWithInterfaces(this IServiceCollection services, IEnumerable<Type> interfaceTypes, IEnumerable<Assembly> fromAssemblies)
    {
        fromAssemblies.SelectMany(x => x.DefinedTypes)
            .Distinct()
            .Where(x => x.IsClass && !x.IsAbstract && (interfaceTypes.Any(y => x.IsAssignableTo(y)) || interfaceTypes.Any(y => x.HasGenericInterface(y))))
            .ToList()
            .ForEach(x => services.AddScoped(x));
        return services;
    }
}