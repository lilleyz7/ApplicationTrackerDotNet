﻿using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ApplicationTracker.Redis
{
    public class RedisCacheService : IRedisService
    {
        private readonly IDistributedCache? _cache;

        public RedisCacheService(IDistributedCache? cache)
        {
            _cache = cache;
        }

        public T? GetData<T>(string key)
        {
            var data = _cache?.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            _cache?.SetString(key, JsonSerializer.Serialize(value), options);
        }
    }
}
