using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MediatorEndpoint.Metadata;

public interface IEndpointProvider
{
    IReadOnlyList<Endpoint> Resolve(MediatorEndpointConfiguration configuration);
}
