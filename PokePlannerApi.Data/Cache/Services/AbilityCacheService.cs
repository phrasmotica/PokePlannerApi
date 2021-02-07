using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of ability resources in the cache.
    /// </summary>
    public class AbilityCacheService : NamedCacheServiceBase<Ability>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public AbilityCacheService(
            INamedCacheSource<Ability> cacheSource,
            IPokeAPI pokeApi,
            ILogger<AbilityCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
