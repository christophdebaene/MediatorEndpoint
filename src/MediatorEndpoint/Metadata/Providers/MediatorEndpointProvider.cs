using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace MediatorEndpoint.Metadata.Providers;

public class MediatorEndpointProvider : IEndpointProvider
{
    public IReadOnlyList<Endpoint> Resolve(MediatorEndpointConfiguration configuration)
    {
        var endpoints = new List<Endpoint>();

        foreach (var type in configuration.AssembliesToRegister.GetTypes(configuration.RequestEvaluator))
        {
            var interfaceType = type.GetInterfaceOrDefault("Mediator", "IRequest`1", "IQuery`1", "ICommand`1");
            if (interfaceType is not null)
            {
                endpoints.Add(new Endpoint
                {
                    Name = configuration.RequestName(type),
                    Kind = configuration.RequestKind(type),
                    RequestType = type,
                    ResponseType = interfaceType.GetGenericArguments().SingleOrDefault(type => type.FullName != "Mediator.Unit")
                });
            }
        }

        return endpoints;
    }
}