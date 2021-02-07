using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of pokedex resources in the cache.
    /// </summary>
    public class PokedexCacheService : NamedCacheServiceBase<Pokedex>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PokedexCacheService(
            INamedCacheSource<Pokedex> cacheSource,
            IPokeAPI pokeApi,
            ILogger<PokedexCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
