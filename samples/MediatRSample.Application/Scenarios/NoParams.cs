using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Scenarios;

[Query]
public record NoParams : IRequest<string>
{
}
public class NoParamsHandler : IRequestHandler<NoParams, string>
{
    public Task<string> Handle(NoParams request, CancellationToken cancellationToken)
    {
        return Task.FromResult("NoParams response");
    }
}