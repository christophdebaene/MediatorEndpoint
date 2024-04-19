using Mediator;
using MediatorEndpoint;

namespace Sample.Application.Scenarios;

[Query]
public record NoResponseWithUnit(Guid Id) : ICommand<Unit>
{
}
public class NoResponseWithUnitHandler : ICommandHandler<NoResponseWithUnit, Unit>
{
    public ValueTask<Unit> Handle(NoResponseWithUnit request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Id);
        return ValueTask.FromResult(Unit.Value);
    }
}