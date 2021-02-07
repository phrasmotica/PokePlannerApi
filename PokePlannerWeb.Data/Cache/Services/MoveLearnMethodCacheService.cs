using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of move learn method resources in the cache.
    /// </summary>
    public class MoveLearnMethodCacheService : NamedCacheServiceBase<MoveLearnMethod>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MoveLearnMethodCacheService(
            INamedCacheSource<MoveLearnMethod> cacheSource,
            IPokeAPI pokeApi,
            ILogger<MoveLearnMethodCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
