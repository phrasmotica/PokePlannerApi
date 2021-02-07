using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
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
