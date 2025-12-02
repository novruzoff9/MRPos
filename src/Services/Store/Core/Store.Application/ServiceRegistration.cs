using Microsoft.Extensions.DependencyInjection;
using Store.Application.Common.Behaviors;
using System.Reflection;

namespace Store.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Mapster config
        var config = new TypeAdapterConfig();
        config.Scan(Assembly.GetExecutingAssembly());

        // DI registrations
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }
}
