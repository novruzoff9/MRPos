using Branches.Grpc;
using FluentValidation;
using IdentityServer.Configurations;
using IdentityServer.Context;
using IdentityServer.Context.Seed;
using IdentityServer.Grpc.Interfaces;
using IdentityServer.Grpc.Services;
using IdentityServer.Helpers;
using IdentityServer.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Shared.Extensions;
using Shared.Middlewares;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true)
    .AddEnvironmentVariables();


builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Add gRPC services to the DI container
builder.Services.AddGrpc();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
    options.ListenAnyIP(5005, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<JwtTokenGenerator>();

// Register gRPC client
builder.Services.AddGrpcClient<BranchesGrpc.BranchesGrpcClient>(options =>
{
    var branchesServiceUrl = builder.Configuration["GrpcSettings:BranchesServiceUrl"];
    options.Address = new Uri(branchesServiceUrl!);
});
builder.Services.AddScoped<IBranchesGrpc, BranchesGrpcImplementation>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureAuth(builder.Configuration);

// Mapster config
TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
await IdentityDbSeeder.SeedAsync(context);

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization(); 
app.MapGrpcService<IdentityGrpcService>();

app.MapControllers();

app.Run();
