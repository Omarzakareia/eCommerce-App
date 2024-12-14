using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Services;

namespace Talabat.Service;
public class ResponseCacheService : IResponseCacheService
{
    private readonly IDatabase _database;

    // Constructor that initializes the Redis database connection
    public ResponseCacheService(IConnectionMultiplexer redis)
    {
       _database = redis.GetDatabase();
    }

    /// Caches the given response object with the specified key and expiration time.
    /// cacheKey: The key used to store the cached response.
    /// response: The response object to be cached. If null, the method returns without caching.
    /// ExpireTime: The duration for which the cached response should remain in the cache.
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan ExpireTime)
    {
        if (response is null) return;
        // the frontend => camel case not a pascal case 
        // so we need to change from pascal case to camel case
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var serializedResponse = JsonSerializer.Serialize(response , options);
        await _database.StringSetAsync(cacheKey, serializedResponse, ExpireTime);

    }


    /// summary: Retrieves the cached response for the specified key.
    /// cacheKey: The key associated with the cached response.
    /// returns The cached response as a JSON string, or null if not found.
    public async Task<string?> GetCachedResponse(string cacheKey)
    {
        var cachedResponse = await _database.StringGetAsync(cacheKey);
        if (cachedResponse.IsNullOrEmpty) return null;

        return cachedResponse;
    }
}
