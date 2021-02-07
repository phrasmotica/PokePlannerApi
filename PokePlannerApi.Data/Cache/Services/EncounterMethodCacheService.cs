using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of encounter method resources in the cache.
    /// </summary>
    public class EncounterMethodCacheService : NamedCacheServiceBase<EncounterMethod>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterMethodCacheService(
            INamedCacheSource<EncounterMethod> cacheSource,
            IPokeAPI pokeApi,
            ILogger<EncounterMethodCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
