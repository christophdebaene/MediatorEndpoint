using MediatorEndpoint.JsonRpc.Internal;
using MediatorEndpoint.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc;
public record JsonRpcRequest
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Id { get; set; }

    [JsonPropertyName("jsonrpc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string JsonRpcVersion { get; set; }

    [JsonPropertyName("method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string Method { get; set; }

    [JsonPropertyName("params")]
    public JsonElement Params { get; set; }

    public static async ValueTask<JsonRpcRequest?> BindAsync(HttpContext context)
    {
        var body = await context.Request.GetBodyOrFormStringAsync();

        var errorResponse = TryParse(body, context.GetSerializerOptions(), out JsonRpcRequest? request);
        if (errorResponse is null)
        {
            var endpoints = context.RequestServices.GetRequiredService<IEndpointCollection>();
            if (!endpoints.Exist(request!.Method))
            {
                errorResponse = JsonRpcErrorResponse.MethodNotFound(request.Id, request.Method);
            }
        }

        if (errorResponse is null)
        {
            return request;
        }
        else
        {
            context.Items[nameof(JsonRpcErrorResponse)] = errorResponse;
            return null;
        }
    }
    public static JsonRpcErrorResponse? TryParse(string? content, JsonSerializerOptions settings, out JsonRpcRequest? request)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            request = null;
            return JsonRpcErrorResponse.InvalidRequest(null, "Body is empty");
        }
        try
        {
            request = JsonSerializer.Deserialize<JsonRpcRequest>(content, settings)!;

            if (string.IsNullOrWhiteSpace(request.Id))
            {
                return JsonRpcErrorResponse.InvalidRequest(null, $"Property 'id' is empty or missing");
            }

            if (string.IsNullOrWhiteSpace(request.JsonRpcVersion))
            {
                return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'jsonrpc' is empty or missing");
            }

            if (!request.JsonRpcVersion.Equals("2.0", StringComparison.OrdinalIgnoreCase))
            {
                return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'jsonrpc' must be 2.0");
            }

            if (string.IsNullOrWhiteSpace(request.Method))
            {
                return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'method' is empty or missing");
            }

            if (request.Params.ValueKind != JsonValueKind.Undefined)
            {
                if (!(request.Params.ValueKind == JsonValueKind.Object || request.Params.ValueKind == JsonValueKind.Array))
                {
                    return JsonRpcErrorResponse.InvalidRequest(request.Id, $"Property 'params' must be of type object or array");
                }
            }

            return null;
        }
        catch (Exception exc)
        {
            request = null;
            return JsonRpcErrorResponse.ParseError(null, exc);
        }
    }
}