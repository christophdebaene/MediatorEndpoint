using System.Text;

namespace MediatorEndpoint.Responses;
public record TextResponse(string? Content, string? ContentType = null, Encoding? ContentEncoding = null)
{
}