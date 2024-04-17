using System;

namespace MediatorEndpoint.Metadata;

public record Endpoint
{
    public required RequestName Name { get; init; }
    public required RequestKind Kind { get; init; }
    public Type RequestType { get; init; }
    public Type? ResponseType { get; init; }
};
