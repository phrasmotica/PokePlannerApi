using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of machines in the cache.
    /// </summary>
    public class MachineCacheService : CacheServiceBase<Machine>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MachineCacheService(
            ICacheSource<Machine> cacheSource,
            IPokeAPI pokeApi,
            ILogger<MachineCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
