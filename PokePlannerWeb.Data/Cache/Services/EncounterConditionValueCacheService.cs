using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of encounter condition value resources in the cache.
    /// </summary>
    public class EncounterConditionValueCacheService : NamedCacheServiceBase<EncounterConditionValue>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterConditionValueCacheService(
            INamedCacheSource<EncounterConditionValue> cacheSource,
            IPokeAPI pokeApi,
            ILogger<EncounterConditionValueCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
