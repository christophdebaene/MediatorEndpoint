using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediatorEndpoint.JsonRpc.Internal;
internal static class HttpExtensions
{
    public static JsonSerializerOptions GetSerializerOptions(this HttpContext context)
    {
        return context.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
    }
    public static async Task<string?> GetBodyOrFormStringAsync(this HttpRequest request)
    {
        return request.HasFormContentType
           ? request.Form["jsonrpc"].FirstOrDefault()
           : await request.GetRawBodyStringAsync();
    }
    public static async Task<IReadOnlyList<IFormFile>> BindFilesAsync(this HttpRequest request)
    {
        var files = new List<IFormFile>();

        if (request.HasFormContentType)
        {
            var form = await request.ReadFormAsync();

            foreach (var file in form.Files)
            {
                if (file.Length == 0 && string.IsNullOrEmpty(file.FileName))
                {
                    continue;
                }

                files.Add(file);
            }
        }

        return files;
    }
    public static async Task<string> GetRawBodyStringAsync(this HttpRequest request)
    {
        if (!request.Body.CanSeek)
        {
            request.EnableBuffering();
        }

        request.Body.Position = 0;

        var reader = new StreamReader(request.Body, Encoding.UTF8);
        var body = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return body;
    }
}