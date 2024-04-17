using Mediator;

namespace Sample.Application.Calculator;

public record Substract(long X, long Y) : IQuery<long>
{
}
public class SubstractHandler : IQueryHandler<Add, long>
{
    public ValueTask<long> Handle(Add request, CancellationToken cancellationToken) => new(request.X - request.Y);
}
