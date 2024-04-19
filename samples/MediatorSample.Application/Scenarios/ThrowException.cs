using Mediator;
using MediatorEndpoint;

namespace Sample.Application.Scenarios;

[Query]
public record ThrowException : IRequest
{
}
public class ThrowExceptionHandler : IRequestHandler<ThrowException>
{
    public ValueTask<Unit> Handle(ThrowException request, CancellationToken cancellationToken)
    {
        throw new Exception("Custom Exception Message");
    }
}