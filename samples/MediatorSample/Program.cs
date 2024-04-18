using MediatorEndpoint;
using MediatorEndpoint.Metadata.Providers;
using Sample.Api.Endpoints;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblies = new List<Assembly> { Assembly.Load("MediatorSample.Application") }.ToArray();

builder.Services.ConfigureHttpJsonOptions(options =>
{
});

builder.Services.AddMediator();
builder.Services.AddMediatorEndpoint(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
    cfg.RequestName = type => new RequestName("MyApp", type.Namespace!.Split(".").Last(), type.Name);
    cfg.EndpointProvider = new MediatorEndpointProvider();
});

var app = builder.Build();
app.MapJsonRpc();
app.Run();