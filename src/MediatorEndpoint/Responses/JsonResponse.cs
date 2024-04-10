namespace MediatorEndpoint.Responses;
public record JsonResponse(string? Content) : TextResponse(Content, "application/json", null)
{
}