
using Catalog.WebAPI.Infrastructure;

namespace Catalog.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<CustomExceptionHandler>();
        return services;
    }
}
