using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Calculator;

[Query]
public record Add(long X, long Y) : IRequest<long>
{
}
public class AddHandler : IRequestHandler<Add, long>
{
    public Task<long> Handle(Add request, CancellationToken cancellationToken) => Task.FromResult(request.X + request.Y);
}
