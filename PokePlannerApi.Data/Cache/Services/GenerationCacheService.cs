using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of generation resources in the cache.
    /// </summary>
    public class GenerationCacheService : NamedCacheServiceBase<Generation>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GenerationCacheService(
            INamedCacheSource<Generation> cacheSource,
            IPokeAPI pokeApi,
            ILogger<GenerationCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
