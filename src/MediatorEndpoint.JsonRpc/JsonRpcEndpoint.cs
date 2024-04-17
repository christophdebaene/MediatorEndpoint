using MediatorEndpoint.JsonRpc.Internal;
using MediatorEndpoint.Metadata;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc;
public class JsonRpcEndpoint
{
    public Metadata.Endpoint Endpoint { get; init; }
    public JsonRpcRequest Request { get; init; }
    public JsonRpcEndpoint(Metadata.Endpoint endpoint, JsonRpcRequest request)
    {
        Endpoint = endpoint;
        Request = request;
    }
    public async Task<object?> CreateMessage(HttpContext context)
    {
        var serializerOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;

        var request = Request.Params.ValueKind == JsonValueKind.Undefined ?
           Activator.CreateInstance(Endpoint.RequestType) :
           JsonSerializer.Deserialize(Request.Params, Endpoint.RequestType, serializerOptions);

        if (request is IHaveId iHaveId)
            iHaveId.Id = Request.Id;

        if (request is IHaveParams iHaveParams)
            iHaveParams.Params = Request.Params;

        if (request is IFileRequest fileRequest)
        {
            var files = await context.Request.BindFilesAsync();
            fileRequest.Files = files.Select(x => new FileProxy(x)).ToList();
        }

        return request;
    }
}
