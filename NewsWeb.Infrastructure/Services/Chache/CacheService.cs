using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace NewsWeb.Infrastructure.Services.Chache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
        {
            var value = await _cache.GetStringAsync(key, cancellationToken);

            if (value != null && value != "[]")
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<T> SetAsync<T>(string key, T value, CancellationToken cancellationToken)
        {
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            }, cancellationToken);

            return value;
        }
    }
}
