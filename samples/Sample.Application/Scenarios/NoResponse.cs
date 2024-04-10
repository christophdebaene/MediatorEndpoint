using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Scenarios;

[Query]
public record NoResponse(Guid Id) : IRequest
{
}
public class NoResponseHandler : IRequestHandler<NoResponse>
{
    public Task Handle(NoResponse request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Id);
        return Task.CompletedTask;
    }
}