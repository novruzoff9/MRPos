using Shared.Extensions;
using Store.Application;
using Store.GrpcAPI.Services;
using Store.Infrastructure;
using Store.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5020, o =>
    {
        o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

app.MapGrpcService<BranchesService>();

app.Run();
