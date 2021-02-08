using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Data store source for Cosmos DB.
    /// </summary>
    public class CosmosDbDataStoreSource<TEntry> : IDataStoreSource<TEntry> where TEntry : EntryBase
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
        /// The container of entries.
        /// </summary>
        protected Container Container;

        /// <summary>
        /// Create connection to database container.
        /// </summary>
        public CosmosDbDataStoreSource(string connectionString, string privateKey, string databaseName, string collectionName)
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
        /// Returns all entries.
        /// </summary>
        public async Task<IEnumerable<TEntry>> GetAll()
        {
            return await GetAllItems<TEntry>();
        }

        /// <summary>
        /// Returns the first entry that matches the given predicate.
        /// </summary>
        public async Task<TEntry> GetOne(Expression<Func<TEntry, bool>> predicate)
        {
            var entries = await GetAllItems<TEntry>();
            return entries.Where(predicate.Compile()).FirstOrDefault();
        }

        /// <summary>
        /// Creates the given entry and returns it.
        /// </summary>
        public async Task<TEntry> Create(TEntry entry)
        {
            await CreateIfNeeded();

            if (string.IsNullOrEmpty(entry.Id))
            {
                entry.Id = entry.Key.ToString();
            }

            try
            {
                await Container.UpsertItemAsync(entry, GetPartitionKey(entry));
            }
            catch (CosmosException e)
            {
                // TODO: proper error handling
                Console.WriteLine(e.Message);
            }

            return entry;
        }

        /// <summary>
        /// Deletes the first entry that matches the given predicate.
        /// </summary>
        public async Task DeleteOne(Expression<Func<TEntry, bool>> predicate)
        {
            var entry = await GetOne(predicate);
            await Container.DeleteItemAsync<TEntry>(entry.Id, GetPartitionKey(entry));
        }

        /// <summary>
        /// Returns all items in the container of the given type.
        /// </summary>
        private async Task<IEnumerable<T>> GetAllItems<T>()
        {
            await CreateIfNeeded();

            var resources = await Container.GetItemLinqQueryable<T>()
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
        /// Returns a partition key for the given entry.
        /// </summary>
        private static PartitionKey GetPartitionKey(TEntry entry)
        {
            return new PartitionKey(entry.Id);
        }
    }
}
