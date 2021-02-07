using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Models;

namespace PokePlannerWeb.Data.Cache.Abstractions
{
    /// <summary>
    /// Cache source for named API resources in Mongo DB.
    /// </summary>
    public class MongoDbNamedCacheSource<TResource> : MongoDbCacheSource<TResource>, INamedCacheSource<TResource> where TResource : NamedApiResource
    {
        /// <summary>
        /// Create connection to database collection.
        /// </summary>
        public MongoDbNamedCacheSource(
            string connectionString,
            string databaseName,
            string collectionName) : base(connectionString, databaseName, collectionName)
        {
        }

        /// <summary>
        /// Returns the cache entry for the resource with the given name.
        /// </summary>
        public Task<CacheEntry<TResource>> GetCacheEntry(string name)
        {
            var entry = Collection.Find(e => e.Resource.Name == name).FirstOrDefault();
            return Task.FromResult(entry);
        }
    }
}
