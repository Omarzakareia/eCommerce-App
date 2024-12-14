namespace Talabat.Core.Services;
public interface IResponseCacheService
{
    // Cache Data 
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan ExpireTime);
    // Get Cached Data 
    Task<string?> GetCachedResponse(string cacheKey);
}
