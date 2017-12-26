using Microsoft.Extensions.Caching.Memory;
using System;

namespace EP.Services.Caching
{
    public sealed class MemoryCacheService : IMemoryCacheService
    {
        private const int Never = 0;
        private const int Absolute = 1;
        private const int Sliding = 2;

        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetOrAddNeverExpiration<T>(string key, Func<T> acquire)
        {
            return GetOrAdd(key, acquire, Never, 0);
        }

        public T GetOrAddSlidingExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120)
        {
            return GetOrAdd(key, acquire, Sliding, cacheInMinutes);
        }

        public T GetOrAddAbsoluteExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120)
        {
            return GetOrAdd(key, acquire, Absolute, cacheInMinutes);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        private T GetOrAdd<T>(string key, Func<T> acquire, int expiration, int cacheInMinutes)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;
            }

            // Locks get and set internally.
            lock (TypeLock<T>.Lock)
            {
                if (_memoryCache.TryGetValue(key, out value))
                {
                    return value;
                }

                value = acquire();

                if (value != null)
                {
                    _memoryCache.Set(key, value, GetMemoryCacheEntryOptions(expiration, cacheInMinutes));
                }

                return value;
            }
        }

        private static MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int expiration, int cacheInMinutes)
        {
            var memoryOptions = new MemoryCacheEntryOptions();

            switch (expiration)
            {
                case Sliding:
                    memoryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(cacheInMinutes));
                    break;

                case Absolute:
                    memoryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheInMinutes));
                    break;

                case Never:
                default:
                    memoryOptions.SetPriority(CacheItemPriority.NeverRemove);
                    break;
            }

            return memoryOptions;
        }

        private static class TypeLock<T>
        {
            public static object Lock { get; } = new object();
        }
    }
}
