namespace MediatorEndpoint.Responses;

public record JsonResponse(string? Content) : IResponse
{
    public static readonly JsonResponse Null = new("null");
}