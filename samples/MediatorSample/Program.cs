using MediatorEndpoint;
using MediatorEndpoint.Metadata.Providers;
using NScalar;
using Sample.Api.Endpoints;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var assemblies = new List<Assembly> { Assembly.Load("MediatorSample.Application") }.ToArray();

builder.Services.AddHttpClient();
builder.Services.AddMediator();
builder.Services.AddMediatorEndpoint(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
    cfg.RequestName = type => new RequestName("MyApp", type.Namespace!.Split(".").Last(), type.Name);
    cfg.EndpointProvider = new MediatorEndpointProvider();
});

builder.Services.AddJsonRpcOpenApi((config) => { });
var app = builder.Build();
app.UseScalar(cfg =>
{
    cfg.ProxyUrl = "/scalarproxy";
});
app.UseJsonRpcOpenApi();
app.MapJsonRpc();
app.MapScalar();

app.Run();