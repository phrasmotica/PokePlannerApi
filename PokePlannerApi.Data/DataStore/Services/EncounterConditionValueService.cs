using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
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
            ILogger<EncounterConditionValueService> logger) : base(dataStoreSource, pokeApi, logger)
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
