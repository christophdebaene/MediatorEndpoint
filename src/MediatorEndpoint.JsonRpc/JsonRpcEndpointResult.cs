using MediatorEndpoint.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc;
public class JsonRpcEndpointResult(JsonRpcEndpoint? Value, JsonRpcErrorResponse? ErrorResponse) : Result<JsonRpcEndpoint>(Value, ErrorResponse)
{
    public static async ValueTask<JsonRpcEndpointResult> BindAsync(HttpContext context)
    {
        var result = await JsonRpcRequestResult.BindAsync(context);
        if (result.ErrorResponse is not null)
        {
            return new JsonRpcEndpointResult(null, result.ErrorResponse);
        }

        var jsonRpcRequest = result.Value!;

        var catalog = context.RequestServices.GetRequiredService<EndpointCatalog>();
        return new JsonRpcEndpointResult(new JsonRpcEndpoint(catalog[jsonRpcRequest.Method!], jsonRpcRequest), result.ErrorResponse);
    }
}
