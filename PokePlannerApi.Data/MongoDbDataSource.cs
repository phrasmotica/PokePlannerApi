using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data
{
    /// <summary>
    /// Service for managing resource entries in Mongo DB.
    /// </summary>
    public class MongoDbDataSource : IDataSource
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MongoDbDataSource> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MongoDbDataSource(
            string connectionString,
            string databaseName,
            ILogger<MongoDbDataSource> logger)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _logger = logger;
        }

        /// <inheritdoc />
        public Task<TEntry> Create<TEntry>(TEntry entry) where TEntry : EntryBase
        {
            var collection = _database.GetCollection<TEntry>("collectionName");
            collection.InsertOne(entry);
            return Task.FromResult(entry);
        }

        /// <inheritdoc />
        public Task<IEnumerable<TEntry>> GetAll<TEntry>() where TEntry : EntryBase
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<TEntry> Get<TEntry>(Expression<Func<TEntry, bool>> predicate) where TEntry : EntryBase
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Update<TEntry>(int key, TEntry entry) where TEntry : EntryBase
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Delete<TEntry>(Expression<Func<TEntry, bool>> predicate) where TEntry : EntryBase
        {
            throw new NotImplementedException();
        }
    }
}
