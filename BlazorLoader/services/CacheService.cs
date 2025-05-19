using System;
using Microsoft.Extensions.Caching.Memory;

public interface IMemoryCacheService
{
    Task<T> GetOrCreate<T>(string key, Func<Task<T>> createItem, TimeSpan expiration);
    void Set<T>(string key, T value, TimeSpan expiration);
    bool TryGet<T>(string key, out T value);
    void Remove(string key);
}

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetOrCreate<T>(string key, Func<Task<T>> myTask, TimeSpan expiration)
    {
        if (!_cache.TryGetValue(key, out T? value))
        {
            Console.WriteLine("no cache, await the task");
            value = await myTask();
            _cache.Set(key, value, expiration);
        }

        if (value == null)
        {
            throw new Exception("Cache error");
        }
        return value;
    }

    public void Set<T>(string key, T value, TimeSpan expiration)
    {
        _cache.Set(key, value, expiration);
    }

    public bool TryGet<T>(string key, out T value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
