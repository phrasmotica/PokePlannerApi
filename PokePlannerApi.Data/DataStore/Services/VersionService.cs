using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.Extensions;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the version entries in the data store.
    /// </summary>
    public class VersionService : NamedApiResourceServiceBase<Version, VersionEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public VersionService(
            IDataStoreSource<VersionEntry> dataStoreSource,
            IPokeAPI pokeApi,
            VersionCacheService versionCacheService,
            ILogger<VersionService> logger) : base(dataStoreSource, pokeApi, versionCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a version entry for the given version.
        /// </summary>
        protected override Task<VersionEntry> ConvertToEntry(Version version)
        {
            var displayNames = version.Names.Localise();

            return Task.FromResult(new VersionEntry
            {
                Key = version.Id,
                Name = version.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all versions.
        /// </summary>
        public async Task<VersionEntry[]> GetAll()
        {
            var allStats = await UpsertAll();
            return allStats.ToArray();
        }

        #endregion
    }
}
