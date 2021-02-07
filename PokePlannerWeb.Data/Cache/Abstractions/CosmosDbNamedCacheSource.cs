using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Models;

namespace PokePlannerWeb.Data.Cache.Abstractions
{
    /// <summary>
    /// Cache source for named API resources in Cosmos DB.
    /// </summary>
    public class CosmosDbNamedCacheSource<TResource> : CosmosDbCacheSource<TResource>, INamedCacheSource<TResource> where TResource : NamedApiResource
    {
        /// <summary>
        /// Create connection to database container.
        /// </summary>
        public CosmosDbNamedCacheSource(
            string connectionString,
            string privateKey,
            string databaseName,
            string collectionName) : base(connectionString, privateKey, databaseName, collectionName)
        {
        }

        /// <summary>
        /// Returns the cache entry for the resource with the given name.
        /// </summary>
        public async Task<CacheEntry<TResource>> GetCacheEntry(string name)
        {
            var entries = await GetAllItems<TResource>();
            return entries.FirstOrDefault(e => e.Resource.Name == name);
        }
    }
}
