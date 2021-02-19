using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Data store source for Mongo DB.
    /// </summary>
    public class MongoDbDataStoreSource<TEntry> : IDataStoreSource<TEntry> where TEntry : EntryBase
    {
        private readonly IMongoCollection<TEntry> _collection;
        private readonly TimeSpan timeToLive = TimeSpan.FromDays(365);

        /// <summary>
        /// Create connection to database collection.
        /// </summary>
        public MongoDbDataStoreSource(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<TEntry>(collectionName);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntry>> GetAll()
        {
            var entries = _collection.Find(_ => true).ToEnumerable();
            return Task.FromResult(entries);
        }

        /// <inheritdoc />
        public Task<TEntry> GetOne(Expression<Func<TEntry, bool>> predicate)
        {
            var entry = _collection.Find(predicate).FirstOrDefault();
            return Task.FromResult(entry);
        }

        /// <inheritdoc />
        public Task<TEntry> Create(TEntry entry)
        {
            entry.CreationTime = DateTime.UtcNow;
            _collection.InsertOne(entry);
            return Task.FromResult(entry);
        }

        /// <inheritdoc />
        public Task DeleteOne(Expression<Func<TEntry, bool>> predicate)
        {
            _collection.DeleteOne(predicate);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<(bool, TEntry)> HasOne(Expression<Func<TEntry, bool>> predicate)
        {
            var entry = await GetOne(predicate);
            var hasIt = entry != null && entry.CreationTime >= DateTime.UtcNow - timeToLive;
            return (hasIt, entry);
        }
    }
}
