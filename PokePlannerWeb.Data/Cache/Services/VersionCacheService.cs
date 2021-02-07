using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of version resources in the cache.
    /// </summary>
    public class VersionCacheService : NamedCacheServiceBase<Version>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public VersionCacheService(
            INamedCacheSource<Version> cacheSource,
            IPokeAPI pokeApi,
            ILogger<VersionCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
