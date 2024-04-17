using Mediator;

namespace Sample.Application.Calculator;

public record Add(long X, long Y) : IQuery<long>
{
}
public class AddHandler : IQueryHandler<Add, long>
{
    public ValueTask<long> Handle(Add request, CancellationToken cancellationToken) => new(request.X + request.Y);
}
