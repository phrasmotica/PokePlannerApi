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
    /// Service for managing the generation entries in the data store.
    /// </summary>
    public class EvolutionTriggerService : NamedApiResourceServiceBase<EvolutionTrigger, EvolutionTriggerEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EvolutionTriggerService(
            IDataStoreSource<EvolutionTriggerEntry> dataStoreSource,
            IPokeAPI pokeApi,
            EvolutionTriggerCacheService generationCacheService,
            ILogger<EvolutionTriggerService> logger) : base(dataStoreSource, pokeApi, generationCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a evolution trigger entry for the given evolution trigger.
        /// </summary>
        protected override Task<EvolutionTriggerEntry> ConvertToEntry(EvolutionTrigger evolutionTrigger)
        {
            var displayNames = evolutionTrigger.Names.Localise();

            return Task.FromResult(new EvolutionTriggerEntry
            {
                Key = evolutionTrigger.Id,
                Name = evolutionTrigger.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all evolution triggers.
        /// </summary>
        public async Task<EvolutionTriggerEntry[]> GetAll()
        {
            var allEvolutionTriggers = await UpsertAll();
            return allEvolutionTriggers.OrderBy(g => g.Id).ToArray();
        }

        #endregion
    }
}
