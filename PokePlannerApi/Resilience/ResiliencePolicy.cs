using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PokePlannerApi.Settings;
using Polly;
using Polly.Caching.Memory;

namespace PokePlannerApi.Resilience
{
    public static class ResiliencePolicy
    {
        public static IAsyncPolicy CreateResiliencePolicy(PokeApiSettings pokeApiSettings, ILogger logger)
        {
            var cachePolicy = Policy.CacheAsync(
                new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())),
                TimeSpan.FromDays(pokeApiSettings.GraphQlClientCacheEntryLifetimeDays),
                onCacheGet: (ctx, key) =>
                {
                    logger.LogInformation($"Got cache entry for key {key}.");
                },
                onCacheMiss: (ctx, key) =>
                {
                    logger.LogInformation($"No cache entry present for key {key}.");
                },
                onCachePut: (ctx, key) =>
                {
                    logger.LogInformation($"Put cache entry for key {key}.");
                },
                onCacheGetError: (ctx, key, ex) =>
                {
                    logger.LogError($"Error getting cache entry for key {key}! Exception: {ex}");
                },
                onCachePutError: (ctx, key, ex) =>
                {
                    logger.LogError($"Error putting cache entry for key {key}! Exception: {ex}");
                }
            );

            return cachePolicy;
        }
    }
}
