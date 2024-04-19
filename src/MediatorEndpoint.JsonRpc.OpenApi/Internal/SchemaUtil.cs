using MediatorEndpoint.Metadata;
using NJsonSchema;
using NSwag;

namespace MediatorEndpoint.JsonRpc.OpenApi.Internal;
internal static class SchemaUtil
{
    public static OpenApiOperation CreateOperation(Endpoint endpoint, JsonSchema requestSchema, JsonSchema responseSchema)
    {
        return new OpenApiOperation
        {
            Summary = endpoint.Name.Name,
            OperationId = endpoint.Name.ToString(),
            Tags = [endpoint.Name.ServiceName],
            Parameters =
            {
                new OpenApiParameter
                {
                    Kind = OpenApiParameterKind.Body,
                    AllowEmptyValue = false,
                    IsRequired = true,
                    Schema = requestSchema
                }
            },
            Responses =
            {
                {
                    "200",
                    new OpenApiResponse
                    {
                        Description = "Ok",
                        Schema = responseSchema
                    }
                }
            }
        };
    }
    public static JsonSchema CreateRequestSchema(string methodName, JsonSchema paramsSchema)
    {
        return new JsonSchema
        {
            Type = JsonObjectType.Object,
            Properties =
            {
                {
                    "id",
                    new JsonSchemaProperty
                    {
                        Default = "b9df3ee6-c49d-4799-b9ce-d9a9b28f67a9",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "jsonrpc",
                    new JsonSchemaProperty
                    {
                        Default = "2.0",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "method",
                    new JsonSchemaProperty
                    {
                        Default = methodName,
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "params",
                    new JsonSchemaProperty
                    {
                        Reference = paramsSchema
                    }
                },
            }
        };
    }
    public static JsonSchema? CreateResponseSchema(JsonSchema? responseSchema)
    {
        return responseSchema;

        /*
        return new JsonSchema
        {
            Type = JsonObjectType.Object,
            Properties =
            {
                {
                    "id",
                    new JsonSchemaProperty
                    {
                        Default = "b9df3ee6-c49d-4799-b9ce-d9a9b28f67a9",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "jsonrpc",
                    new JsonSchemaProperty
                    {
                        Default = "2.0",
                        Type = JsonObjectType.String,
                        IsRequired = true,
                    }
                },
                {
                    "result",
                    responseSchema is null ?
                        new JsonSchemaProperty
                        {
                            Type = JsonObjectType.Null,
                            IsRequired = true
                        } :
                        new JsonSchemaProperty
                        {
                            Reference = responseSchema
                        }
                }
            }
        };
        */
    }
}
