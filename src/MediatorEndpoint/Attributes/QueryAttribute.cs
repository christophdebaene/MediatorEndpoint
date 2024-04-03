﻿using System;

namespace MediatorEndpoint;

public class QueryAttribute : RequestTypeAttribute
{
    public QueryAttribute() : base(RequestType.Query)
    {
    }
    public static bool Is(Type type) => Get(type) == RequestType.Query;
}
