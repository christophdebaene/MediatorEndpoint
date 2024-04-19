using MediatorEndpoint;
using MediatorEndpoint.Metadata;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatorEndpoint(this IServiceCollection services, Action<MediatorEndpointConfiguration> configuration)
    {
        var serviceConfig = new MediatorEndpointConfiguration();
        configuration.Invoke(serviceConfig);
        return services.AddMediatorEndpoint(serviceConfig);
    }
    public static IServiceCollection AddMediatorEndpoint(this IServiceCollection services, MediatorEndpointConfiguration configuration)
    {
        if (configuration.AssembliesToRegister.Count == 0)
        {
            throw new ArgumentException("No assemblies found to scan. Supply at least one assembly to scan for handlers.");
        }

        var endpoints = new EndpointCollection(configuration.EndpointProvider.Resolve(configuration));

        if (configuration.VerifyRequestKind)
        {
            var request = endpoints.FirstOrDefault(x => x.Kind == RequestKind.Undefined);
            if (request is not null)
            {
                throw new Exception($"Request '{request.RequestType.FullName}' has undefined RequestKind");
            }
        }

        return services.AddSingleton<IEndpointCollection>(endpoints);
    }
}
