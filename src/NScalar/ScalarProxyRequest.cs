using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NScalar;
public class ScalarProxyRequest
{
    [JsonPropertyName("method")]
    [JsonConverter(typeof(HttpMethodJsonConverter))]
    public HttpMethod Method { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
    public static async ValueTask<ScalarProxyRequest?> BindAsync(HttpContext context)
    {
        var request = await context.Request.ReadFromJsonAsync<ScalarProxyRequest>();
        return request;
    }
}

public class HttpMethodJsonConverter : JsonConverter<HttpMethod>
{
    public override HttpMethod? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) ? null : new HttpMethod(value);
    }

    public override void Write(Utf8JsonWriter writer, HttpMethod value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}