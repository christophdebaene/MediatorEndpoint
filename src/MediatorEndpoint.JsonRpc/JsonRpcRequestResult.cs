using MediatorEndpoint.JsonRpc.Internal;
using MediatorEndpoint.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc;
public class JsonRpcRequestResult(JsonRpcRequest? Value, JsonRpcErrorResponse? ErrorResponse) : Result<JsonRpcRequest>(Value, ErrorResponse)
{
    public static async ValueTask<JsonRpcRequestResult> BindAsync(HttpContext context)
    {
        var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
        var body = context.Request.HasFormContentType
            ? context.Request.Form["jsonrpc"].FirstOrDefault()
            : await context.Request.GetRawBodyStringAsync();

        var errorResponse = JsonRpcRequest.TryParse(body, serializerOptions, out JsonRpcRequest? request);

        if (errorResponse is null)
        {
            var catalog = context.RequestServices.GetRequiredService<EndpointCatalog>();
            if (!catalog.Exist(request!.Method))
            {
                errorResponse = JsonRpcErrorResponse.MethodNotFound(request.Id, request.Method);
            }
        }
        return new JsonRpcRequestResult(request, errorResponse);
    }
}
