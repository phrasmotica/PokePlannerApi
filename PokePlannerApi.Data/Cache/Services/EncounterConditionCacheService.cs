using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
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
