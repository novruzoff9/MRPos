using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Interfaces;
using Shared.Services;

namespace Shared.Extensions.Redis;

public static class RedisRegistration
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisConfiguration>(configuration.GetSection("RedisConfiguration"));
        services.AddHttpClient();
        services.AddSingleton<RedisService>(sp =>
        {
            var redisSettings = sp.GetRequiredService<IOptions<RedisConfiguration>>().Value;
            var redisService = new RedisService(redisSettings);
            redisService.Connect();
            return redisService;
        });

        services.AddScoped<IRedisCacheService, RedisCacheService>();
        return services;
    }
}
