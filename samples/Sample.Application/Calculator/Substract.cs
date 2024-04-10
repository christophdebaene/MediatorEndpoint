using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Calculator;

[Query]
public record Substract(long X, long Y) : IRequest<long>
{
}
public class SubstractHandler : IRequestHandler<Substract, long>
{
    public Task<long> Handle(Substract request, CancellationToken cancellationToken) => Task.FromResult(request.X - request.Y);
}
