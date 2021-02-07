using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Models;

namespace PokePlannerWeb.Data.Cache.Abstractions
{
    /// <summary>
    /// Cache source for API resources in Mongo DB.
    /// </summary>
    public class MongoDbCacheSource<TResource> : ICacheSource<TResource> where TResource : ResourceBase
    {
        /// <summary>
        /// The collection of entries.
        /// </summary>
        protected IMongoCollection<CacheEntry<TResource>> Collection;

        /// <summary>
        /// Create connection to database collection.
        /// </summary>
        public MongoDbCacheSource(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            Collection = database.GetCollection<CacheEntry<TResource>>(collectionName);
        }

        /// <summary>
        /// Returns all entries.
        /// </summary>
        public Task<IEnumerable<TResource>> GetAll()
        {
            var entries = Collection.Find(_ => true).ToEnumerable();
            return Task.FromResult(entries.Select(e => e.Resource));
        }

        /// <summary>
        /// Returns the first entry that matches the given predicate.
        /// </summary>
        public Task<TResource> GetOne(Expression<Func<TResource, bool>> predicate)
        {
            var func = predicate.Compile();
            var entry = Collection.Find(e => func(e.Resource)).FirstOrDefault();
            return Task.FromResult(entry.Resource);
        }

        /// <summary>
        /// Returns the cache entry for the resource with the given ID.
        /// </summary>
        public Task<CacheEntry<TResource>> GetCacheEntry(int id)
        {
            var entry = Collection.Find(e => e.Resource.Id == id).FirstOrDefault();
            return Task.FromResult(entry);
        }

        /// <summary>
        /// Creates the given entry and returns it.
        /// </summary>
        public Task<TResource> Create(TResource resource)
        {
            var entry = new CacheEntry<TResource>
            {
                CreationTime = DateTime.UtcNow,
                Resource = resource
            };

            Collection.InsertOne(entry);
            return Task.FromResult(resource);
        }

        /// <summary>
        /// Deletes the first entry that matches the given predicate.
        /// </summary>
        public Task DeleteOne(Expression<Func<TResource, bool>> predicate)
        {
            var func = predicate.Compile();
            Collection.DeleteOne(e => func(e.Resource));
            return Task.CompletedTask;
        }
    }
}
