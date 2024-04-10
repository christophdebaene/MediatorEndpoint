namespace MediatorEndpoint.Responses;
public record FileResponse(string Filename, string ContentType, byte[] Data) : IResponse
{
}
