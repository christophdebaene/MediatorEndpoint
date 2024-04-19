using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatorEndpoint.Metadata.Providers;
internal static class TypeExtensions
{
    public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> filter)
        => assemblies.SelectMany(x => x.GetExportedTypes()).Where(x => !x.IsAbstract && !x.IsInterface && filter(x));

    public static Type? GetInterfaceOrDefault(this Type type, string namespaceName, params string[] interfaces)
        => type.GetInterfaces().Where(x => x.Namespace == namespaceName).FirstOrDefault(x => interfaces.Contains(x.Name));
}
