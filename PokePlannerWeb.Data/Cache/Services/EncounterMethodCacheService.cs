using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
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
