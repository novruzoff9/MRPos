﻿using Organization.WebAPI.Infrastructure;

namespace Organization.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpContextAccessor();

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
