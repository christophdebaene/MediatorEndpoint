using MediatorEndpoint;
using MediatorEndpoint.Metadata;
using MediatorEndpoint.Metadata.Providers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;
public class MediatorEndpointConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    public bool VerifyRequestKind { get; set; } = false;
    public Func<Type, RequestName> RequestName { get; set; } = x => new RequestName(null, null, x.Name);
    public Func<Type, RequestKind> RequestKind { get; set; } = RequestKindAttribute.Get;
    public Func<Type, bool> RequestEvaluator { get; set; } = x => true;
    public IEndpointProvider EndpointProvider { get; set; } = new MediatREndpointProvider();
    public MediatorEndpointConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }
}