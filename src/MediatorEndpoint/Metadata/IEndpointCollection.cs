using System.Collections.Generic;

namespace MediatorEndpoint.Metadata;

public interface IEndpointCollection : IReadOnlyList<Endpoint>
{
    Endpoint this[string name] { get; }
    bool Exist(string name);
}