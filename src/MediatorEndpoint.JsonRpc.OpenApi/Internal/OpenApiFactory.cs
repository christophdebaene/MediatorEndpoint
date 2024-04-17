using MediatorEndpoint.Metadata;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using System.Data;

namespace MediatorEndpoint.JsonRpc.OpenApi;
internal class OpenApiFactory
{
    public static OpenApiDocument Create(IEndpointCollection endpoints, OpenApiConfiguration configuration)
    {
        var document = new OpenApiDocument
        {
            SchemaType = NJsonSchema.SchemaType.OpenApi3
        };

        var jsonSchemaFactory = new JsonSchemaFactory(document, configuration.Settings);
        jsonSchemaFactory.ScanCatalog(endpoints);

        document.Info = new OpenApiInfo
        {
            Title = "API Specification",
            Description = "API Description"
        };

        document.Produces.Add("application/json");
        document.Consumes.Add("application/json");

        foreach (var request in endpoints.OrderBy(x => x.Name.ServiceName))
        {
            document.Paths.Add($"/{request.Name.ServiceName}/{request.Name.Name}", new OpenApiPathItem
            {
                {
                    OpenApiOperationMethod.Post,
                    CreateOperation(configuration, jsonSchemaFactory, request)
                }
            });
        }

        return document;
    }
    internal static OpenApiOperation CreateOperation(OpenApiConfiguration configuration, JsonSchemaFactory jsonSchemaFactory, Endpoint request)
    {
        return new OpenApiOperation
        {
            Summary = $"{request.Name.Name}",
            OperationId = request.Name.ToString(),
            Parameters =
            {
                new OpenApiParameter
                {
                    Kind = OpenApiParameterKind.Body,
                    AllowEmptyValue = false,
                    IsRequired = true,
                    Schema = jsonSchemaFactory.CreateJsonRpcRequest(request)
                }
            },
            Responses =
            {
                {
                    "200",
                    new OpenApiResponse
                    {
                        Description = "Ok",
                        Schema = jsonSchemaFactory.CreateJsonRpcResponse(request)
                    }
                }
            },
            Tags = [request.Name.ServiceName],
        };
    }
}
