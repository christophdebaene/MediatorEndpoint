using Mediator;
using MediatorEndpoint;

namespace Sample.Application.Scenarios;

[Query]
public record NoParams : IRequest<string>
{
}
public class NoParamsHandler : IRequestHandler<NoParams, string>
{
    public ValueTask<string> Handle(NoParams request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult("NoParams response");
    }
}