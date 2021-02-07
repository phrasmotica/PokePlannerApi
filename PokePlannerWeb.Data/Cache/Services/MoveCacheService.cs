using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of move resources in the cache.
    /// </summary>
    public class MoveCacheService : NamedCacheServiceBase<Move>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveCacheService(
            INamedCacheSource<Move> cacheSource,
            IPokeAPI pokeApi,
            ILogger<MoveCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
