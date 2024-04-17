using MediatorEndpoint.JsonRpc.OpenApi;
using MediatorEndpoint.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using NSwag;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenApiExtensions
{
    public static IServiceCollection AddJsonRpcOpenApi(this IServiceCollection services, Action<OpenApiConfiguration>? configuration)
    {
        var config = new OpenApiConfiguration();
        configuration?.Invoke(config);
        return services.AddJsonRpcOpenApi(config);
    }
    internal static IServiceCollection AddJsonRpcOpenApi(this IServiceCollection services, OpenApiConfiguration configuration)
    {
        var jsonOptions = services.AddScoped((serviceProvider) =>
        {
            var serializerOptions = serviceProvider.GetService<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>()!.Value.SerializerOptions;
            serializerOptions ??= new System.Text.Json.JsonSerializerOptions
            {
            };

            if (configuration.JsonSerializerOptionsPostProcess is not null)
                configuration.JsonSerializerOptionsPostProcess(serializerOptions);

            configuration.Settings.SerializerOptions = serializerOptions;

            var endpoints = serviceProvider.GetRequiredService<IEndpointCollection>();

            var openApiDocument = OpenApiFactory.Create(endpoints, configuration);

            if (configuration.PostProcess is not null)
                configuration.PostProcess(openApiDocument);

            return openApiDocument;
        });

        return services;
    }
    public static IEndpointRouteBuilder UseJsonRpcOpenApi(this IEndpointRouteBuilder endpoints, string routePrefix = "/openapi")
    {
        endpoints.MapGet(routePrefix, ([FromServices] OpenApiDocument openApiDocument, [FromQuery] string format = "json") =>
        {
            var openApi = format == "json" ? openApiDocument.ToJson() : openApiDocument.ToYaml();
            return format == "json" ? Results.Text(openApi, "application/json") : Results.Text(openApi, "text/plain");
        });

        return endpoints;
    }
}
