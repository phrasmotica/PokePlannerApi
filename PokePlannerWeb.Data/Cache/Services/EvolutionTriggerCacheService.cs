using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of evolution trigger resources in the cache.
    /// </summary>
    public class EvolutionTriggerCacheService : NamedCacheServiceBase<EvolutionTrigger>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EvolutionTriggerCacheService(
            INamedCacheSource<EvolutionTrigger> cacheSource,
            IPokeAPI pokeApi,
            ILogger<EvolutionTriggerCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
