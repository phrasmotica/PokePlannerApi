using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of move category resources in the cache.
    /// </summary>
    public class MoveCategoryCacheService : NamedCacheServiceBase<MoveCategory>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveCategoryCacheService(
            INamedCacheSource<MoveCategory> cacheSource,
            IPokeAPI pokeApi,
            ILogger<MoveCategoryCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
