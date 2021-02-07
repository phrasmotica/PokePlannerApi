using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Abstractions;

namespace PokePlannerWeb.Data.Cache.Services
{
    /// <summary>
    /// Service for managing the collection of type resources in the cache.
    /// </summary>
    public class TypeCacheService : NamedCacheServiceBase<Type>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public TypeCacheService(
            INamedCacheSource<Type> cacheSource,
            IPokeAPI pokeApi,
            ILogger<TypeCacheService> logger) : base(cacheSource, pokeApi, logger)
        {
        }
    }
}
