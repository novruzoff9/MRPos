using Shared.ResultTypes;

namespace Shared.Interfaces;

public interface IRedisCacheService
{
    Task<Response<bool>> DeleteAsync(string key);
    Task<Response<T>> GetAsync<T>(string Id);
    Task<Response<string>> SetAsync<T>(string key, T value, TimeSpan? expiry = null);
}