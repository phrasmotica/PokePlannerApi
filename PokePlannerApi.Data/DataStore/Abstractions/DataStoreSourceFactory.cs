using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Conventions;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.Util;

namespace PokePlannerApi.Data.DataStore.Abstractions
{
    /// <summary>
    /// Factory for data store sources.
    /// </summary>
    public class DataStoreSourceFactory
    {
        /// <summary>
        /// The connection string to the database instance.
        /// </summary>
        private readonly string ConnectionString = EnvHelper.GetVariable("PokePlannerApiConnectionString");

        /// <summary>
        /// The private key of the database.
        /// </summary>
        private readonly string PrivateKey = EnvHelper.GetVariable("PokePlannerApiPrivateKey");

        /// <summary>
        /// The name of the database.
        /// </summary>
        private readonly string DatabaseName = "PokePlannerApiDataStore";

        /// <summary>
        /// Creates an entry source for the given entry type.
        /// </summary>
        public IDataStoreSource<TEntry> Create<TEntry>(string collectionName) where TEntry : EntryBase
        {
            var isCosmosDb = Regex.IsMatch(ConnectionString, @"https:\/\/[\w-]+\.documents\.azure\.com");
            if (isCosmosDb)
            {
                return new CosmosDbDataStoreSource<TEntry>(ConnectionString, PrivateKey, DatabaseName, collectionName);
            }

            ConfigureMongoDb();
            return new MongoDbDataStoreSource<TEntry>(ConnectionString, DatabaseName, collectionName);
        }

        /// <summary>
        /// Configures settings for Mongo DB.
        /// </summary>
        private void ConfigureMongoDb()
        {
            // ignore null values of certain types
            ConventionRegistry.Register(
                "IgnoreIfDefault",
                new ConventionPack
                {
                    new IgnoreIfDefaultConvention(true)
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
