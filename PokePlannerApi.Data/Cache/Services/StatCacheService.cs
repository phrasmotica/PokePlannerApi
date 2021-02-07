using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of stat resources in the cache.
    /// </summary>
    public class StatCacheService : NamedCacheServiceBase<Stat>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public StatCacheService(
            INamedCacheSource<Stat> cacheSource,
            IPokeAPI pokeApi,
            ILogger<StatCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
