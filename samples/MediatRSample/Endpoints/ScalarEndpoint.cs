using MediatorEndpoint.JsonRpc;
using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NScalar;

namespace Sample.Api.Endpoints;
public static class ScalarEndpoint
{
    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapPost("/scalarproxy", async (HttpContext context, ISender sender, ScalarProxyRequest scalarProxy, CancellationToken cancellationToken) =>
        {
            if (scalarProxy.Method == HttpMethod.Post && scalarProxy.Data.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
                var httpClientFactory = context.RequestServices.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient();
                client.BaseAddress = new UriBuilder(context.Request.Scheme, context.Request.Host.Host, context.Request.Host.Port ?? -1).Uri;

                var error = JsonRpcRequest.TryParse(scalarProxy.Data.ToString(), serializerOptions, out JsonRpcRequest? request);
                if (error is not null)
                {
                    return Results.BadRequest(error);
                }

                var response = await client.PostAsJsonAsync("/jsonrpc", scalarProxy.Data, cancellationToken);
                var data = await response.Content.ReadAsStringAsync(cancellationToken);
                return ScalarProxyResults.Ok(data);
            }

            throw new NotImplementedException();
        });

        return app;
    }
}

