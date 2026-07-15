using Microsoft.Extensions.Caching.Distributed;
using RealTimeChatApp.Application.Contracts.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealTimeChatApp.Infrastructure.Caching
{
    public sealed class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RedisCacheService(
            IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(
            string key,
            CancellationToken cancellationToken = default)
        {
            var json = await _cache.GetStringAsync(
                key,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(
                json,
                JsonOptions);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiration,
            CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var json = JsonSerializer.Serialize(
                value,
                JsonOptions);

            await _cache.SetStringAsync(
                key,
                json,
                options,
                cancellationToken);
        }

        public async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(
                key,
                cancellationToken);
        }
    }
}
