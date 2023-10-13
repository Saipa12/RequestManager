using System.Reflection;

namespace RequestManager.Core.Extensions;

public static class TypeExtensions
{
    public static bool HasGenericInterface(this Type type, Type interfaceType)
        => type.GetTypeInfo().ImplementedInterfaces.Any(x => x.IsGenericType(interfaceType));

    public static bool IsGenericType(this Type type, Type genericType)
        => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == genericType;
}