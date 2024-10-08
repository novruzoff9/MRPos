﻿
using Catalog.WebAPI.Infrastructure;
using Shared.Services;

namespace Catalog.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpContextAccessor();

        services.AddScoped<ISharedIdentityService, SharedIdentityService>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });
        return services;
    }
}
