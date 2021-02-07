using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of Pokemon resources in the cache.
    /// </summary>
    public class PokemonCacheService : NamedCacheServiceBase<Pokemon>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PokemonCacheService(
            INamedCacheSource<Pokemon> cacheSource,
            IPokeAPI pokeApi,
            ILogger<PokemonCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
