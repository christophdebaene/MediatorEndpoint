using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatorEndpoint.Metadata.Providers;
internal static class TypeExtensions
{
    public static IEnumerable<Type> GetTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> filter)
        => assemblies.SelectMany(x => x.GetExportedTypes()).Where(x => filter(x));

    public static bool ImplementsInterface(this Type type, string fullNameInterface)
        => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.FullName == fullNameInterface);
}
