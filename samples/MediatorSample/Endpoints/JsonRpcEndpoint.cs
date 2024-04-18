using Mediator;
using MediatorEndpoint.JsonRpc;

namespace Sample.Api.Endpoints;
public static class JsonRpcEndpoint
{
    public static WebApplication MapJsonRpc(this WebApplication app)
    {
        app.MapPost("/jsonrpc", async (HttpContext context, ISender sender, JsonRpc? jsonRpc, CancellationToken cancellationToken) =>
        {
            var id = jsonRpc!.Request.Id;

            try
            {
                var message = await jsonRpc!.CreateMessage(context);
                Console.WriteLine(message ?? "".ToString());

                var response = await sender.Send(message, cancellationToken);
                return JsonRpcResults.Response(id, response);
            }
            catch (Exception exc)
            {
                return JsonRpcResults.Response(JsonRpcErrorResponse.InternalError(id, exc));
            };
        })
        .AddEndpointFilter<JsonRpcValidationFilter>();

        return app;
    }
}
