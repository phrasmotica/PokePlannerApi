using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of move damage class resources in the cache.
    /// </summary>
    public class MoveDamageClassCacheService : NamedCacheServiceBase<MoveDamageClass>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveDamageClassCacheService(
            INamedCacheSource<MoveDamageClass> cacheSource,
            IPokeAPI pokeApi,
            ILogger<MoveDamageClassCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
