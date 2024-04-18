using System;

namespace MediatorEndpoint.Metadata;

public record Endpoint
{
    public required RequestName Name { get; init; }
    public required RequestKind Kind { get; init; }
    public required Type RequestType { get; init; }
    public required Type? ResponseType { get; init; }
};
