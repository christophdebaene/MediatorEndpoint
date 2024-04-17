using System;
using System.Reflection;

namespace MediatorEndpoint;

[AttributeUsage(AttributeTargets.Class)]
public class RequestKindAttribute(RequestKind kind) : Attribute
{
    public RequestKind Kind { get; } = kind;
    public static RequestKind Get(Type type)
    {
        var attr = type.GetCustomAttribute<RequestKindAttribute>(false);
        return attr == null ? RequestKind.Undefined : attr.Kind;
    }
}
