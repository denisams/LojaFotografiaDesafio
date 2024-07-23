using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Catalogo.Infrastructure.Repositories
{
    public abstract class CachedRepository<T>
    {
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30); // Duração do cache de 30 minutos

        protected CachedRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        protected async Task<T> GetOrAddAsync(string cacheKey, Func<Task<T>> getItemCallback)
        {
            var cacheData = await _cache.GetStringAsync(cacheKey);
            if (cacheData != null)
            {
                return JsonConvert.DeserializeObject<T>(cacheData);
            }

            var data = await getItemCallback();
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return data;
        }

        protected async Task<IEnumerable<T>> GetOrAddAsync(string cacheKey, Func<Task<IEnumerable<T>>> getItemsCallback)
        {
            var cacheData = await _cache.GetStringAsync(cacheKey);
            if (cacheData != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(cacheData);
            }

            var data = await getItemsCallback();
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheDuration
            });

            return data;
        }

        protected async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }
    }
}
