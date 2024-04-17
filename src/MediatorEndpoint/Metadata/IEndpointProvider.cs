using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace MediatorEndpoint.Metadata;

public interface IEndpointProvider
{
    IEnumerable<Endpoint> Resolve(MediatorEndpointConfiguration configuration);
}
