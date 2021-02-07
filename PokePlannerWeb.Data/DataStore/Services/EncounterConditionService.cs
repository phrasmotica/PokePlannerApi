using System.Collections.Generic;
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
    /// Service for managing the encounter condition entries in the data store.
    /// </summary>
    public class EncounterConditionService : NamedApiResourceServiceBase<EncounterCondition, EncounterConditionEntry>
    {
        /// <summary>
        /// The encounter condition value cache service.
        /// </summary>
        private readonly EncounterConditionValueService EncounterConditionValueService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EncounterConditionService(
            IDataStoreSource<EncounterConditionEntry> dataStoreSource,
            IPokeAPI pokeApi,
            EncounterConditionCacheService encounterConditionCacheService,
            EncounterConditionValueService encounterConditionValueService,
            ILogger<EncounterConditionService> logger) : base(dataStoreSource, pokeApi, encounterConditionCacheService, logger)
        {
            EncounterConditionValueService = encounterConditionValueService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a location area entry for the given location area.
        /// </summary>
        protected override async Task<EncounterConditionEntry> ConvertToEntry(EncounterCondition condition)
        {
            var displayNames = condition.Names.Localise();
            var values = await GetValues(condition);

            return new EncounterConditionEntry
            {
                Key = condition.Id,
                Name = condition.Name,
                DisplayNames = displayNames.ToList(),
                Values = values.ToList()
            };
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Returns the values for the given encounter condition.
        /// </summary>
        private async Task<IEnumerable<EncounterConditionValueEntry>> GetValues(EncounterCondition condition)
        {
            var valueList = new List<EncounterConditionValueEntry>();

            foreach (var res in condition.Values)
            {
                var value = await EncounterConditionValueService.Upsert(res);
                valueList.Add(value);
            }

            return valueList;
        }

        #endregion
    }
}
