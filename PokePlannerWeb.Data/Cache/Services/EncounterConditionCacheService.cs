using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of encounter condition resources in the cache.
    /// </summary>
    public class EncounterConditionCacheService : NamedCacheServiceBase<EncounterCondition>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterConditionCacheService(
            INamedCacheSource<EncounterCondition> cacheSource,
            IPokeAPI pokeApi,
            ILogger<EncounterConditionCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
