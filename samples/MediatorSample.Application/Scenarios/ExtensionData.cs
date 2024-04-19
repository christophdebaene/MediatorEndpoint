using Mediator;
using MediatorEndpoint;
using System.Text;
using System.Text.Json.Serialization;

namespace Sample.Application.Scenarios;

[Query]
public record ExtensionData : IRequest<string>
{
    public string X { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> Data { get; set; }
}
public class ExtensionDataHandler : IRequestHandler<ExtensionData, string>
{
    public ValueTask<string> Handle(ExtensionData request, CancellationToken cancellationToken)
    {
        var sb = new StringBuilder();
        sb.AppendLine("X:" + request.X?.ToString());

        if (request.Data is not null)
        {
            foreach (var key in request.Data.Keys)
            {
                sb.AppendLine(key + ":" + request.Data[key].ToString());
            }
        }

        return ValueTask.FromResult(sb.ToString());
    }
}