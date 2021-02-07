using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of evolution chain resources in the cache.
    /// </summary>
    public class EvolutionChainCacheService : CacheServiceBase<EvolutionChain>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EvolutionChainCacheService(
            ICacheSource<EvolutionChain> cacheSource,
            IPokeAPI pokeApi,
            ILogger<EvolutionChainCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
