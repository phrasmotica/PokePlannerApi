using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Conventions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Factory for data store sources.
    /// </summary>
    public class DataStoreSourceFactory
    {
        /// <summary>
        /// Creates an entry source for the given entry type.
        /// </summary>
        public IDataStoreSource<TEntry> Create<TEntry>(
            string collectionName,
            string databaseName,
            string privateKey,
            string connectionString) where TEntry : EntryBase
        {
            var isCosmosDb = Regex.IsMatch(connectionString, @"https:\/\/[\w-]+\.documents\.azure\.com");
            if (isCosmosDb)
            {
                return new CosmosDbDataStoreSource<TEntry>(connectionString, privateKey, databaseName, collectionName);
            }

            var isMongoDb = Regex.IsMatch(connectionString, @"mongodb://");
            if (isMongoDb)
            {
                ConfigureMongoDb();
                return new MongoDbDataStoreSource<TEntry>(connectionString, databaseName, collectionName);
            }

            return new NullDataStoreSource<TEntry>();
        }

        /// <summary>
        /// Configures settings for Mongo DB.
        /// </summary>
        private static void ConfigureMongoDb()
        {
            // ignore null values of certain types
            ConventionRegistry.Register(
                "IgnoreIfNull",
                new ConventionPack
                {
                    new IgnoreIfNullConvention(true)
                },
                t => true
            );

            // ignore extra values of certain types
            ConventionRegistry.Register(
                "IgnoreExtra",
                new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true)
                },
                t => true
            );
        }
    }
}
