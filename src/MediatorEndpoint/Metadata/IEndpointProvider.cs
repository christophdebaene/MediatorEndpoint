using MediatorEndpoint.DependencyInjection;
using System.Collections.Generic;

namespace MediatorEndpoint.Metadata;

public interface IEndpointProvider
{
    IEnumerable<EndpointInfo> Resolve(MediatorEndpointConfiguration configuration);
}
