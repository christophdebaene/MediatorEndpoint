using MediatorEndpoint.JsonRpc.Internal;
using MediatorEndpoint.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc;
public class JsonRpc
{
    public Metadata.Endpoint Endpoint { get; init; }
    public JsonRpcRequest Request { get; init; }
    public JsonRpc(Metadata.Endpoint endpoint, JsonRpcRequest request)
    {
        Endpoint = endpoint;
        Request = request;
    }
    public static async ValueTask<JsonRpc?> BindAsync(HttpContext context)
    {
        var jsonRpcRequest = await JsonRpcRequest.BindAsync(context);
        if (jsonRpcRequest is null)
        {
            return null;
        }

        var endpoints = context.RequestServices.GetRequiredService<IEndpointCollection>();
        return new JsonRpc(endpoints[jsonRpcRequest.Method!], jsonRpcRequest);
    }
    public async Task<object?> CreateMessageAsync(HttpContext context)
    {
        var request = Request.Params.ValueKind == JsonValueKind.Undefined ?
           Activator.CreateInstance(Endpoint.RequestType) :
           JsonSerializer.Deserialize(Request.Params, Endpoint.RequestType, context.GetSerializerOptions());

        if (request is IFileRequest fileRequest)
        {
            var files = await context.Request.BindFilesAsync();
            fileRequest.Files = files.Select(x => new FileProxy(x)).ToList();
        }

        return request;
    }
}
