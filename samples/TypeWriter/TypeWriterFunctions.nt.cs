using NTypewriter.CodeModel;
using NTypewriter.CodeModel.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TypeWriter
{
    public record MediatorRequest(RequestName Name, IType Request, IType? Response);
    public record RequestName(string? Namespace, string? ServiceName, string Name)
    {
        public override string ToString()
        {
            string?[] parts = [Namespace, ServiceName, Name];
            return string.Join(".", parts.Where(x => !string.IsNullOrWhiteSpace(x)));
        }
    }
    public static class TypeWriterConfig
    {
        public static RequestName RequestNameProvider(IClass @class) => new("Sample.Application", @class.Namespace.Split('.').Last(), @class.BareName);
        public static bool TypeFilter(IType type) => type.Namespace.StartsWith("Sample.Application");
    }
    public static class TypeWriterFunctions
    {
        public static string ToTsEnumValues(this IEnum @enum) => string.Join(" | ", @enum.Values.Select(x => $"'{x.Name}'"));
        public static string ToTsType(this IType? type) => type is null ? "void" : type.ToTypeScriptType();
        public static string ClassBase(this IClass @class)
        {
            return (@class.IsFileRequest(), @class.HasBaseClass) switch
            {
                (true, true) => $": {@class.BaseClass}, IFileRequest",
                (true, false) => $": IFileRequest",
                (false, true) => $": {@class.BaseClass}",
                _ => string.Empty
            };
        }
        public static bool IsFileRequest(this IClass @class) => @class.Interfaces.Any(x => x.BareName == "IFileRequest");
        public static IEnumerable<IEnum> IsEnum(this IEnumerable<IType> types) => types.OfType<IEnum>();
        public static IEnumerable<IClass> IsClass(this IEnumerable<IType> types) => types.OfType<IClass>();
        public static IEnumerable<IType> AllReferencedTypes(this IEnumerable<MediatorRequest> requests) => requests.Select(x => x.Request).ScanReferencedTypes(TypeWriterConfig.TypeFilter);
        public static IEnumerable<MediatorRequest> MediatorRequests(this IEnumerable<IType> types)
        {
            foreach (var type in types.OfType<IClass>().Where(x => !x.IsInterface && !x.IsAbstract))
            {
                var request = type.Interfaces.FirstOrDefault(x => x.Namespace == "MediatR" && x.BareName == "IRequest");
                if (request is not null)
                {
                    var requestName = TypeWriterConfig.RequestNameProvider(type);

                    yield return request.TypeArguments.Count() == 1
                        ? new MediatorRequest(requestName, type, request.TypeArguments.First())
                        : new MediatorRequest(requestName, type, null);
                }
            }
        }
        public static IEnumerable<IGrouping<string, MediatorRequest>> MediatorRequestsByService(this IEnumerable<MediatorRequest> requests) => requests.GroupBy(x => x.Name.ServiceName);
        public static IEnumerable<IType> ScanReferencedTypes(this IType type, Func<IType, bool> typeFilter)
        {
            var hashSet = new HashSet<IType>(new TypeComparer());
            var referencedTypes = type.AllReferencedTypes();

            foreach (var referencedType in referencedTypes)
            {
                if (!hashSet.Contains(referencedType) && typeFilter(referencedType))
                {
                    hashSet.Add(referencedType);
                    hashSet.UnionWith(referencedType.ScanReferencedTypes(typeFilter));
                }
            }

            hashSet.Add(type);
            return hashSet;
        }
        public static IEnumerable<IType> ScanReferencedTypes(this IEnumerable<IType> types, Func<IType, bool> typeFilter)
        {
            var hashSet = new HashSet<IType>(new TypeComparer());
            foreach (var type in types)
            {
                if (!hashSet.Contains(type) && typeFilter(type))
                {
                    hashSet.UnionWith(type.ScanReferencedTypes(typeFilter));
                    hashSet.Add(type);
                }
            }

            return hashSet.ToList();
        }
    }
    public class TypeComparer : IEqualityComparer<IType>
    {
        public bool Equals(IType x, IType y) => x.FullName.Equals(y.FullName);
        public int GetHashCode(IType obj) => obj.FullName.GetHashCode();
    }
}

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}