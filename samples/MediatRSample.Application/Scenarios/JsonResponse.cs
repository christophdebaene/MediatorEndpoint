using MediatorEndpoint;
using MediatorEndpoint.Responses;
using MediatR;

namespace Sample.Application.Scenarios;

[Query]
public record JsonResponseRequest : IRequest<JsonResponse>
{
}
public class JsonResponseRequestHandler : IRequestHandler<JsonResponseRequest, JsonResponse>
{
    public Task<JsonResponse> Handle(JsonResponseRequest request, CancellationToken cancellationToken)
    {
        var payload = """
        {
            "Id": "12345"
        }
        """;

        return Task.FromResult(new JsonResponse(payload));
    }
}