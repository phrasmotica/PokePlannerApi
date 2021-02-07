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
    public class GenerationService : NamedApiResourceServiceBase<Generation, GenerationEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GenerationService(
            IDataStoreSource<GenerationEntry> dataStoreSource,
            IPokeAPI pokeApi,
            GenerationCacheService generationCacheService,
            ILogger<GenerationService> logger) : base(dataStoreSource, pokeApi, generationCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a generation entry for the given generation.
        /// </summary>
        protected override Task<GenerationEntry> ConvertToEntry(Generation generation)
        {
            var displayNames = generation.Names.Localise();

            return Task.FromResult(new GenerationEntry
            {
                Key = generation.Id,
                Name = generation.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all generations.
        /// </summary>
        public async Task<GenerationEntry[]> GetAll()
        {
            var allGenerations = await UpsertAll();
            return allGenerations.OrderBy(g => g.Id).ToArray();
        }

        /// <summary>
        /// Returns the generation of the given version group.
        /// </summary>
        public async Task<GenerationEntry> GetByVersionGroup(VersionGroup versionGroup)
        {
            return await Upsert(versionGroup.Generation);
        }

        #endregion
    }
}
