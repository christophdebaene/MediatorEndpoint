using System;
using System.Text.Json.Serialization;

namespace MediatorEndpoint.JsonRpc;
public record JsonRpcErrorResponse
{
    [JsonPropertyName("id")]
    [JsonPropertyOrder(0)]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string? Id { get; init; }

    [JsonPropertyName("jsonrpc")]
    [JsonPropertyOrder(1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string JsonRpcVersion { get; private set; } = "2.0";

    [JsonPropertyName("error")]
    [JsonPropertyOrder(2)]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required JsonRpcError Error { get; init; }

    public static JsonRpcErrorResponse Create(string? id, int code, string errorMessage, object? data = null) => new()
    {
        Id = id ?? Guid.Empty.ToString(),
        Error = new JsonRpcError(code, errorMessage, data)
    };

    public static JsonRpcErrorResponse InvalidRequest(string? id, string message) => Create(id, JsonRpcErrorCode.InvalidRequest, message);
    public static JsonRpcErrorResponse MethodNotFound(string? id, string? methodName) => Create(id, JsonRpcErrorCode.MethodNotFound, $"Method '{methodName}' does not exist");
    public static JsonRpcErrorResponse ParseError(string? id, Exception exc) => Create(id, JsonRpcErrorCode.ParseError, exc.Message, exc.Data);
    public static JsonRpcErrorResponse InternalError(string? id, Exception exc) => Create(id, JsonRpcErrorCode.InternalError, exc.Message, exc.Data);
}