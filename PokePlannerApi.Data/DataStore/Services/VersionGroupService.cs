using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the version group entries in the data store.
    /// </summary>
    public class VersionGroupService : NamedApiResourceServiceBase<VersionGroup, VersionGroupEntry>
    {
        /// <summary>
        /// The generations service.
        /// </summary>
        private readonly GenerationService GenerationsService;

        /// <summary>
        /// The pokedexes service.
        /// </summary>
        private readonly PokedexService PokedexesService;

        /// <summary>
        /// The versions service.
        /// </summary>
        private readonly VersionService VersionsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public VersionGroupService(
            IDataStoreSource<VersionGroupEntry> dataStoreSource,
            IPokeAPI pokeApi,
            GenerationService generationsService,
            PokedexService pokedexesService,
            VersionService versionsService,
            ILogger<VersionGroupService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            GenerationsService = generationsService;
            PokedexesService = pokedexesService;
            VersionsService = versionsService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a version group entry for the given version group.
        /// </summary>
        protected override async Task<VersionGroupEntry> ConvertToEntry(VersionGroup versionGroup)
        {
            var displayNames = await GetDisplayNames(versionGroup);
            var generation = await GenerationsService.GetByVersionGroup(versionGroup);
            var versions = await VersionsService.UpsertMany(versionGroup.Versions);
            var pokedexes = await PokedexesService.UpsertMany(versionGroup.Pokedexes);

            return new VersionGroupEntry
            {
                Key = versionGroup.Id,
                Name = versionGroup.Name,
                Order = versionGroup.Order,
                DisplayNames = displayNames.ToList(),
                Generation = new Generation
                {
                    Id = generation.GenerationId,
                    Name = generation.Name
                },
                Versions = versions.ToList(),
                Pokedexes = pokedexes.Select(p => new Pokedex
                {
                    Id = p.PokedexId,
                    Name = p.Name
                }).ToList()
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the index of the oldest version group.
        /// </summary>
        public async Task<int> GetOldestVersionGroupId()
        {
            var entries = await GetAllEntries();
            return entries.Select(vg => vg.VersionGroupId).Min();
        }

        /// <summary>
        /// Returns the index of the newest version group.
        /// </summary>
        public async Task<int> GetNewestVersionGroupId()
        {
            var entries = await GetAllEntries();
            return entries.Select(vg => vg.VersionGroupId).Max();
        }

        /// <summary>
        /// Returns all version groups.
        /// </summary>
        public async Task<VersionGroupEntry[]> GetAll()
        {
            var allVersionGroups = await UpsertAll();
            return allVersionGroups.OrderBy(vg => vg.Order).ToArray();
        }

        /// <summary>
        /// Returns the version groups spanned by the set of version IDs.
        /// </summary>
        public async Task<VersionGroupEntry[]> UpsertManyByVersionIds(IEnumerable<int> versionIds)
        {
            var allVersionGroups = await UpsertAll();
            var relevantVersionGroups = allVersionGroups.Where(vg =>
            {
                var myVersionIds = vg.Versions.Select(v => v.VersionId);
                return myVersionIds.Intersect(versionIds).Any();
            });

            return relevantVersionGroups.ToArray();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the display names of the given version group in all locales.
        /// </summary>
        private async Task<IEnumerable<LocalString>> GetDisplayNames(VersionGroup versionGroup)
        {
            var versions = await VersionsService.UpsertMany(versionGroup.Versions);
            var versionsNames = versions.Select(v => v.DisplayNames.OrderBy(n => n.Language).ToList());
            var namesList = versionsNames.Aggregate(
                (nv1, nv2) => nv1.Zip(
                    nv2, (n1, n2) => new LocalString
                    {
                        Language = n1.Language,
                        Value = n1.Value + "/" + n2.Value
                    }
                ).ToList()
            );

            return namesList;
        }

        #endregion
    }
}
