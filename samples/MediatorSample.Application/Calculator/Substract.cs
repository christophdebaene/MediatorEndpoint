using Mediator;

namespace Sample.Application.Calculator;

public record Substract(long X, long Y) : IRequest<long>
{
}
public class SubstractHandler : IRequestHandler<Substract, long>
{
    public ValueTask<long> Handle(Substract request, CancellationToken cancellationToken) => ValueTask.FromResult(request.X - request.Y);
}
