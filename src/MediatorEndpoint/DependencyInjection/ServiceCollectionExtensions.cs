using MediatorEndpoint.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MediatorEndpoint.DependencyInjection;
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

        var endpoints = configuration.EndpointProvider.Resolve(configuration).ToList();

        if (configuration.VerifyRequestType)
        {
            foreach (var request in endpoints.Where(x => RequestTypeAttribute.Get(x.RequestType) == RequestType.Unknown))
                throw new System.Exception($"Request {request} is unknown");
        }

        var typeCatalog = new EndpointCatalog(endpoints);
        return services.AddSingleton(typeCatalog);
    }
}
