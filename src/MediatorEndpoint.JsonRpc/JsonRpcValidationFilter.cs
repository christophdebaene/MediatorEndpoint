using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc;

public class JsonRpcValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.Items.TryGetValue(nameof(JsonRpcErrorResponse), out object? value))
        {
            return Results.Json(value);
        }

        return await next(context);
    }
}
