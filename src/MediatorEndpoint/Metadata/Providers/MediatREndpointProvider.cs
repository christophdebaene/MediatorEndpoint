using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatorEndpoint.Metadata.Providers;

public class MediatREndpointProvider : IEndpointProvider
{
    public IEnumerable<Endpoint> Resolve(MediatorEndpointConfiguration configuration)
    {
        return configuration.AssembliesToRegister
            .GetTypes(configuration.RequestEvaluator)
            .Where(x => x.ImplementsInterface("MediatR.IBaseRequest"))
            .Select(x => new Endpoint
            {
                Name = configuration.RequestName(x),
                Kind = configuration.RequestKind(x),
                RequestType = x,
                ResponseType = GetResponseType(x)
            });
    }
    static Type? GetResponseType(Type requestType)
    {
        var requestInterface = requestType.GetInterface("IRequest`1");
        return requestInterface?.GetGenericArguments().SingleOrDefault(type => type.FullName != "MediatR.Unit");
    }
}
