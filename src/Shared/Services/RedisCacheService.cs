using Shared.Extensions.Redis;
using Shared.Interfaces;
using Shared.ResultTypes;
using StackExchange.Redis;
using System.Text.Json;

namespace Shared.Services;


public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RedisCacheService(RedisService redisService)
    {
        _database = redisService.GetDb();
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false
        };
    }
    public async Task<Response<bool>> DeleteAsync(string key)
    {
        bool result = await _database.KeyDeleteAsync(key);
        return result ? Response<bool>.Success(200) : Response<bool>.Fail("Failed to delete cache", 400);
    }

    public async Task<Response<T>> GetAsync<T>(string Id)
    {
        var jsonData = await _database.StringGetAsync(Id);
        if (jsonData.IsNullOrEmpty)
            return Response<T>.Fail("Cache not found", 404);
        var jsonString = jsonData.ToString();
        return Response<T>.Success(JsonSerializer.Deserialize<T>(jsonString), 200);
    }

    public async Task<Response<string>> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var jsonData = JsonSerializer.Serialize(value, _jsonSerializerOptions);
        ExpirationExtensions.TrySetDefaultExpiry(ref expiry);
        bool result = await _database.StringSetAsync(key, jsonData, expiry);
        return result ?
           Response<string>.Success($"Value successfully written to cache with key: {key}", 200) :
           Response<string>.Fail("Failed to set cache", 400);
    }

    private class ExpirationExtensions
    {
        private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(30);
        public static void TrySetDefaultExpiry(ref TimeSpan? expiry)
        {
            if (expiry == null)
            {
                expiry = DefaultExpiry;
            }
        }
    }
}
