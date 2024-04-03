using MediatorEndpoint.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MediatorEndpoint.DependencyInjection;
public class MediatorEndpointConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    public bool VerifyRequestType { get; set; } = true;
    public Func<Type, RequestName> RequestName { get; set; } = x => new RequestName(null, null, x.Name);
    public IEndpointProvider EndpointProvider { get; set; } = new MediatREndpointProvider();
    public MediatorEndpointConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }
}