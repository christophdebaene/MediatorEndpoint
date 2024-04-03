using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Calculator;

[Query]
public class Substract : IRequest<long>
{
    public long X { get; set; }
    public long Y { get; set; }
}
public class SubstractHandler : IRequestHandler<Substract, long>
{
    public Task<long> Handle(Substract request, CancellationToken cancellationToken) => Task.FromResult(request.X - request.Y);
}
