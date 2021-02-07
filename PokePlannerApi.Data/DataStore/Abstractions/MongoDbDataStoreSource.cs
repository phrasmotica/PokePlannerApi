using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using PokePlannerApi.Data.DataStore.Models;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Data store source for Mongo DB.
    /// </summary>
    public class MongoDbDataStoreSource<TEntry> : IDataStoreSource<TEntry> where TEntry : EntryBase
    {
        /// <summary>
        /// The collection of entries.
        /// </summary>
        protected IMongoCollection<TEntry> Collection;

        /// <summary>
        /// Create connection to database collection.
        /// </summary>
        public MongoDbDataStoreSource(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            Collection = database.GetCollection<TEntry>(collectionName);
        }

        /// <summary>
        /// Returns all entries.
        /// </summary>
        public Task<IEnumerable<TEntry>> GetAll()
        {
            var entries = Collection.Find(_ => true).ToEnumerable();
            return Task.FromResult(entries);
        }

        /// <summary>
        /// Returns the first entry that matches the given predicate.
        /// </summary>
        public Task<TEntry> GetOne(Expression<Func<TEntry, bool>> predicate)
        {
            var entry = Collection.Find(predicate).FirstOrDefault();
            return Task.FromResult(entry);
        }

        /// <summary>
        /// Creates the given entry and returns it.
        /// </summary>
        public Task<TEntry> Create(TEntry entry)
        {
            Collection.InsertOne(entry);
            return Task.FromResult(entry);
        }

        /// <summary>
        /// Deletes the first entry that matches the given predicate.
        /// </summary>
        public Task DeleteOne(Expression<Func<TEntry, bool>> predicate)
        {
            Collection.DeleteOne(predicate);
            return Task.CompletedTask;
        }
    }
}
