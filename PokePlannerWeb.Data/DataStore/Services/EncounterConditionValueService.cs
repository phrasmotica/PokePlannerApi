using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Services;
using PokePlannerWeb.Data.DataStore.Abstractions;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.Extensions;

namespace PokePlannerWeb.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the encounter condition value entries in the data store.
    /// </summary>
    public class EncounterConditionValueService : NamedApiResourceServiceBase<EncounterConditionValue, EncounterConditionValueEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterConditionValueService(
            IDataStoreSource<EncounterConditionValueEntry> dataStoreSource,
            IPokeAPI pokeApi,
            EncounterConditionValueCacheService encounterConditionValueCacheService,
            ILogger<EncounterConditionValueService> logger) : base(dataStoreSource, pokeApi, encounterConditionValueCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns an encounter condition value entry for the given encounter condition value.
        /// </summary>
        protected override Task<EncounterConditionValueEntry> ConvertToEntry(EncounterConditionValue condition)
        {
            var displayNames = condition.Names.Localise();

            return Task.FromResult(new EncounterConditionValueEntry
            {
                Key = condition.Id,
                Name = condition.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion
    }
}
