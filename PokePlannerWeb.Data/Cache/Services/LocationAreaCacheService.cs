using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of location area resources in the cache.
    /// </summary>
    public class LocationAreaCacheService : NamedCacheServiceBase<LocationArea>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public LocationAreaCacheService(
            INamedCacheSource<LocationArea> cacheSource,
            IPokeAPI pokeApi,
            ILogger<LocationAreaCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
