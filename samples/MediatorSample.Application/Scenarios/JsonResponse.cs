using Mediator;
using MediatorEndpoint;
using MediatorEndpoint.Responses;

namespace Sample.Application.Scenarios;

[Query]
public record JsonResponseRequest : IRequest<JsonResponse>
{
}
public class JsonResponseRequestHandler : IRequestHandler<JsonResponseRequest, JsonResponse>
{
    public ValueTask<JsonResponse> Handle(JsonResponseRequest request, CancellationToken cancellationToken)
    {
        var payload = """
        {
            "Id": "12345"
        }
        """;

        return ValueTask.FromResult(new JsonResponse(payload));
    }
}