using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Models;

namespace PokePlannerWeb.Data.Cache.Abstractions
{
    /// <summary>
    /// Cache source for API resources in Cosmos DB.
    /// </summary>
    public class CosmosDbCacheSource<TResource> : ICacheSource<TResource> where TResource : ResourceBase
    {
        /// <summary>
        /// The client to Cosmos DB.
        /// </summary>
        protected CosmosClient CosmosClient;

        /// <summary>
        /// The database name.
        /// </summary>
        protected readonly string DatabaseName;

        /// <summary>
        /// The container name.
        /// </summary>
        protected readonly string ContainerName;

        /// <summary>
        /// The container of resources.
        /// </summary>
        protected Container Container;

        /// <summary>
        /// Create connection to database container.
        /// </summary>
        public CosmosDbCacheSource(string connectionString, string privateKey, string databaseName, string collectionName)
        {
            CosmosClient = new CosmosClient(connectionString, privateKey, new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    IgnoreNullValues = true
                }
            });

            DatabaseName = databaseName;
            ContainerName = collectionName;
        }

        /// <summary>
        /// Returns all resources.
        /// </summary>
        public async Task<IEnumerable<TResource>> GetAll()
        {
            var entries = await GetAllItems<TResource>();
            return entries.Select(e => e.Resource);
        }

        /// <summary>
        /// Returns the first resource that matches the given predicate.
        /// </summary>
        public async Task<TResource> GetOne(Expression<Func<TResource, bool>> predicate)
        {
            var resources = await GetAll();
            return resources.Where(predicate.Compile()).FirstOrDefault();
        }

        /// <summary>
        /// Returns the cache entry for the resource with the given ID.
        /// </summary>
        public async Task<CacheEntry<TResource>> GetCacheEntry(int id)
        {
            var entries = await GetAllItems<TResource>();
            return entries.FirstOrDefault(e => e.Resource.Id == id);
        }

        /// <summary>
        /// Returns the first resource that matches the given predicate.
        /// </summary>
        protected async Task<CacheEntry<TResource>> GetOneCacheEntry(Func<TResource, bool> predicate)
        {
            var entries = await GetAllItems<TResource>();
            return entries.Where(e => predicate(e.Resource)).FirstOrDefault();
        }

        /// <summary>
        /// Creates the given resource and returns it.
        /// </summary>
        public async Task<TResource> Create(TResource resource)
        {
            await CreateIfNeeded();

            var entry = new CacheEntry<TResource>
            {
                Id = resource.Id.ToString(),
                CreationTime = DateTime.UtcNow,
                Resource = resource
            };

            try
            {
                await Container.UpsertItemAsync(entry, GetPartitionKey(entry));
            }
            catch (CosmosException e)
            {
                // TODO: proper error handling
                Console.WriteLine(e.Message);
            }

            return resource;
        }

        /// <summary>
        /// Deletes the first cache entry that matches the given predicate.
        /// </summary>
        public async Task DeleteOne(Expression<Func<TResource, bool>> predicate)
        {
            var entry = await GetOneCacheEntry(predicate.Compile());
            await Container.DeleteItemAsync<CacheEntry<TResource>>(entry.Resource.Id.ToString(), GetPartitionKey(entry));
        }

        /// <summary>
        /// Returns all items in the container of the given type.
        /// </summary>
        protected async Task<IEnumerable<CacheEntry<T>>> GetAllItems<T>() where T : ResourceBase
        {
            await CreateIfNeeded();

            var resources = await Container.GetItemLinqQueryable<CacheEntry<T>>()
                                           .ToFeedIterator()
                                           .ReadNextAsync();

            return resources.Resource;
        }

        /// <summary>
        /// Creates the database and container if needed.
        /// </summary>
        protected async Task CreateIfNeeded()
        {
            var databaseResponse = await CosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseName);
            var database = databaseResponse.Database;

            var containerProperties = new ContainerProperties(ContainerName, "/id");
            var containerResponse = await database.CreateContainerIfNotExistsAsync(containerProperties);
            Container = containerResponse.Container;
        }

        /// <summary>
        /// Returns a partition key for the given resource.
        /// </summary>
        protected static PartitionKey GetPartitionKey(CacheEntry<TResource> entry)
        {
            return new PartitionKey(entry.Id.ToString());
        }
    }
}
