using MediatorEndpoint.Metadata;
using MediatorEndpoint.Responses;
using NJsonSchema;
using NJsonSchema.Generation;
using NSwag;

namespace MediatorEndpoint.JsonRpc.OpenApi;
internal class JsonSchemaResolver(OpenApiDocument document, JsonSchemaGeneratorSettings settings)
{
    private readonly JsonSchemaGenerator _generator = new(settings);
    private readonly OpenApiSchemaResolver _resolver = new(document, settings);
    public JsonSchema? GetOrCreate(Type? type)
    {
        if (type is null)
            return null;

        if (type == typeof(JsonResponse))
        {
            return new JsonSchema
            {
                Type = JsonObjectType.Object
            };
        }

        return _generator.Generate(type, _resolver);
    }
    public void Scan(IEndpointCollection endpoints)
    {
        foreach (var endpoint in endpoints)
        {
            GetOrCreate(endpoint.RequestType);
            if (endpoint.ResponseType is not null)
                GetOrCreate(endpoint.ResponseType);
        }
    }
}