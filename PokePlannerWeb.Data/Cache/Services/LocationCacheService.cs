using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of location resources in the cache.
    /// </summary>
    public class LocationCacheService : NamedCacheServiceBase<Location>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocationCacheService(
            INamedCacheSource<Location> cacheSource,
            IPokeAPI pokeApi,
            ILogger<LocationCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
