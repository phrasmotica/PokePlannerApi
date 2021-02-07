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
    /// Service for managing the encounter method entries in the data store.
    /// </summary>
    public class EncounterMethodService : NamedApiResourceServiceBase<EncounterMethod, EncounterMethodEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterMethodService(
            IDataStoreSource<EncounterMethodEntry> dataStoreSource,
            IPokeAPI pokeApi,
            EncounterMethodCacheService encounterConditionValueCacheService,
            ILogger<EncounterMethodService> logger) : base(dataStoreSource, pokeApi, encounterConditionValueCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns an encounter method entry for the given encounter method.
        /// </summary>
        protected override Task<EncounterMethodEntry> ConvertToEntry(EncounterMethod method)
        {
            var displayNames = method.Names.Localise();

            return Task.FromResult(new EncounterMethodEntry
            {
                Key = method.Id,
                Name = method.Name,
                Order = method.Order,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion
    }
}
