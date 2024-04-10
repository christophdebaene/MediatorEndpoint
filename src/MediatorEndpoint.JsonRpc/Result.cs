namespace MediatorEndpoint.JsonRpc;

public abstract class Result<T>(T? Value, JsonRpcErrorResponse? ErrorResponse)
{
    public bool IsValid => ErrorResponse is null;
    public JsonRpcErrorResponse? ErrorResponse { get; } = ErrorResponse;
    public T? Value { get; } = Value;
}
