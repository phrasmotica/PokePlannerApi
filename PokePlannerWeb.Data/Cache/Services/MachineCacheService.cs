using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
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
