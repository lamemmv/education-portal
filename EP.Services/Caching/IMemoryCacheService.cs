using System;

namespace EP.Services.Caching
{
    public interface IMemoryCacheService
    {
        T GetOrAddNeverExpiration<T>(string key, Func<T> acquire);

        T GetOrAddSlidingExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120);

        T GetOrAddAbsoluteExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120);

        void Remove(string key);
    }
}
