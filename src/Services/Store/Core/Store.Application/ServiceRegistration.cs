using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Store.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));

        // Mapster config
        var config = new TypeAdapterConfig();
        config.Scan(Assembly.GetExecutingAssembly());

        // DI registrations
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}
