using Identity.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Application.Common.Interfaces;
using Store.Infrastructure.Grpc.Services;

namespace Store.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IIdentityGrpcClient, IdentityGrpcClient>();
        services.AddGrpcClient<IdentityGrpc.IdentityGrpcClient>(cfg =>
        {
            string identityGrpcService = configuration["Services:IdentityGrpcService"] ?? "http://localhost:5003";
            cfg.Address = new Uri(identityGrpcService);
        });
    }
}
