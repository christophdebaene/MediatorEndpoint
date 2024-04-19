using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace MediatorEndpoint.Metadata.Providers;

public class MediatREndpointProvider : IEndpointProvider
{
    public IReadOnlyList<Endpoint> Resolve(MediatorEndpointConfiguration configuration)
    {
        var endpoints = new List<Endpoint>();

        foreach (var type in configuration.AssembliesToRegister.GetTypes(configuration.RequestEvaluator))
        {
            var interfaceType = type.GetInterfaceOrDefault("MediatR", "IRequest", "IRequest`1");
            if (interfaceType is not null)
            {
                endpoints.Add(new Endpoint
                {
                    Name = configuration.RequestName(type),
                    Kind = configuration.RequestKind(type),
                    RequestType = type,
                    ResponseType = interfaceType.GetGenericArguments().SingleOrDefault(type => type.FullName != "MediatR.Unit")
                });
            }
        }

        return endpoints;
    }
}
