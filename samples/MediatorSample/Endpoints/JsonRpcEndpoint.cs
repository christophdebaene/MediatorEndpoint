using Mediator;
using MediatorEndpoint.JsonRpc;

namespace MediatorSample.Endpoints;
public static class JsonRpcEndpoint
{
    public static WebApplication MapJsonRpc(this WebApplication app)
    {
        app.MapPost("/jsonrpc", async (HttpContext context, ISender sender, JsonRpcEndpointResult endpointResult, CancellationToken cancellationToken) =>
        {
            if (!endpointResult.IsValid)
            {
                return JsonRpcResults.Response(endpointResult.ErrorResponse);
            }

            var endpoint = endpointResult.Value!;

            try
            {
                var message = await endpoint.CreateMessage(context);
                Console.WriteLine(message.ToString());
                var response = await sender.Send(message, cancellationToken);
                return JsonRpcResults.Response(endpoint.Request.Id, response);
            }
            catch (Exception exc)
            {
                var errorResponse = new JsonRpcErrorResponse
                {
                    Id = endpoint.Request.Id,
                    Error = new JsonRpcError(JsonRpcErrorCode.InvalidRequest, exc.Message, exc.Data)
                };

                return JsonRpcResults.Response(errorResponse);
            };
        });

        return app;
    }
}
