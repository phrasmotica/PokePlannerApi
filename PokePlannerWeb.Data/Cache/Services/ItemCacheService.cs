using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
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
