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
                TimeSpan.FromDays(pokeApiSettings.ResiliencePolicyCacheEntryLifetimeDays),
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

            var waitAndRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
                pokeApiSettings.ResiliencePolicyRetryCount,
                sleepDurationProvider: count => TimeSpan.FromSeconds(Math.Pow(count, 2)),
                onRetry: (ex, ts, count, ctx) =>
                {
                    logger.LogInformation($"Wait-and-retry caught exception for operation {ctx.OperationKey}: {ex.Message}. Waiting {ts.TotalSeconds} seconds before retry {count}");
                }
            );

            return Policy.WrapAsync(cachePolicy, waitAndRetryPolicy);
        }
    }
}
