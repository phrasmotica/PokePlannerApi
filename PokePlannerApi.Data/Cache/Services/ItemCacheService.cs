using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of item resources in the cache.
    /// </summary>
    public class ItemCacheService : NamedCacheServiceBase<Item>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemCacheService(
            INamedCacheSource<Item> cacheSource,
            IPokeAPI pokeApi,
            ILogger<ItemCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
