using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Abstractions;

namespace PokePlannerApi.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of Pokemon form resources in the cache.
    /// </summary>
    public class PokemonFormCacheService : NamedCacheServiceBase<PokemonForm>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PokemonFormCacheService(
            INamedCacheSource<PokemonForm> cacheSource,
            IPokeAPI pokeApi,
            ILogger<PokemonFormCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
