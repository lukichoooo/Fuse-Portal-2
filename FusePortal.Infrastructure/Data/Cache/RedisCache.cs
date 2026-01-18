using FusePortal.Application.Interfaces.Services;
using FusePortal.Infrastructure.Settings.Cache;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FusePortal.Infrastructure.Data.Cache
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _db;
        private readonly RedisSettings _settings;

        public RedisCache(
            IConnectionMultiplexer redis,
            IOptions<RedisSettings> options)
        {
            _db = redis.GetDatabase();
            _settings = options.Value;

            if (_settings.DefaultMinutes <= 0)
                throw new InvalidOperationException("Redis TTL must be > 0");
        }

        public async Task<string?> GetValueAsync(string key)
            => await _db.StringGetAsync(key);

        public async Task SetValueAsync(string key, string value)
            => await _db.StringSetAsync(
                    key,
                    value,
                    TimeSpan.FromMinutes(_settings.DefaultMinutes));
    }
}
