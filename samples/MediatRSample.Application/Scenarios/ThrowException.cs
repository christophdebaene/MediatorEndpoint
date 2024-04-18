using MediatorEndpoint;
using MediatR;

namespace Sample.Application.Scenarios;

[Query]
public record ThrowException : IRequest
{
}
public class ThrowExceptionHandler : IRequestHandler<ThrowException>
{
    public Task Handle(ThrowException request, CancellationToken cancellationToken)
    {
        throw new Exception("Custom Exception Message");
    }
}