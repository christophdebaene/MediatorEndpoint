using System.Text.Json.Serialization;

namespace MediatorEndpoint.JsonRpc;
public record JsonRpcError
{
    [JsonPropertyName("code")]
    [JsonPropertyOrder(0)]
    public int Code { get; init; }

    [JsonPropertyName("message")]
    [JsonPropertyOrder(1)]
    public string Message { get; init; }

    [JsonPropertyName("data")]
    [JsonPropertyOrder(2)]
    public object? Data { get; init; }

    public JsonRpcError(int code, string message, object? data = null)
    {
        Code = code;
        Message = message;
        Data = data;
    }
}
