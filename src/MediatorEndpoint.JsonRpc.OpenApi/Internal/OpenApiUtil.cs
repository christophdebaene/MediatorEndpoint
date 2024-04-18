using MediatorEndpoint.JsonRpc.OpenApi.Internal;
using MediatorEndpoint.Metadata;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using System.Data;

namespace MediatorEndpoint.JsonRpc.OpenApi;
internal class OpenApiUtil
{
    public static OpenApiDocument Create(IEndpointCollection endpoints, OpenApiConfiguration configuration)
    {
        var document = new OpenApiDocument
        {
            SchemaType = NJsonSchema.SchemaType.OpenApi3
        };

        var schemaResolver = new JsonSchemaResolver(document, configuration.Settings);
        schemaResolver.Scan(endpoints);

        document.Info = new OpenApiInfo
        {
            Title = "API Specification",
            Description = "API Description"
        };

        document.Produces.Add("application/json");
        document.Consumes.Add("application/json");

        foreach (var endpoint in endpoints.OrderBy(x => x.Name.ServiceName))
        {
            var requestSchema = JsonRpcUtil.CreateRequestSchema(endpoint.Name.ToString(), schemaResolver.GetOrCreate(endpoint.RequestType));
            var responseSchema = JsonRpcUtil.CreateResponseSchema(schemaResolver.GetOrCreate(endpoint.ResponseType));

            if (responseSchema is not null)
            {
                document.Paths.Add($"/{endpoint.Name.ServiceName}/{endpoint.Name.Name}", new OpenApiPathItem
                {
                    {
                        OpenApiOperationMethod.Post,
                        JsonRpcUtil.CreateOperation(endpoint, requestSchema, responseSchema)
                    }
                });
            }
        }

        return document;
    }
}
