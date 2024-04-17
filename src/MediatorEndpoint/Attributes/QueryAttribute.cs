using System;

namespace MediatorEndpoint;

public class QueryAttribute : RequestKindAttribute
{
    public QueryAttribute() : base(RequestKind.Query)
    {
    }
    public static bool Is(Type type) => Get(type) == RequestKind.Query;
}
