using System;

namespace EP.Services.Caching
{
    public interface IMemoryCacheService
    {
        T GetNeverExpiration<T>(string key, Func<T> acquire);

        T GetSlidingExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120);

        T GetAbsoluteExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120);

        void Remove(string key);
    }
}
