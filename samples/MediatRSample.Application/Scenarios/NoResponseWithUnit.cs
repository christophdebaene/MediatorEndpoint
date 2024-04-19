using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Scenarios;

[Query]
public record NoResponseWithUnit(Guid Id) : IRequest<Unit>
{
}
public class NoResponseWithUnitHandler : IRequestHandler<NoResponseWithUnit, Unit>
{
    public Task<Unit> Handle(NoResponseWithUnit request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Id);
        return Task.FromResult(Unit.Value);
    }
}