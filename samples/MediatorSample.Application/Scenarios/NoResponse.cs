using Mediator;

namespace Sample.Application.Scenarios;

public record NoResponse(Guid Id) : ICommand
{
}
public class NoResponseHandler : ICommandHandler<NoResponse>
{
    public ValueTask<Unit> Handle(NoResponse request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Id);
        return ValueTask.FromResult(Unit.Value);
    }
}