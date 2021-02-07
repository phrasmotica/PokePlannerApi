using System.Collections.Generic;
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
    /// Service for managing the ability entries in the data store.
    /// </summary>
    public class AbilityService : NamedApiResourceServiceBase<Ability, AbilityEntry>
    {
        /// <summary>
        /// The version group service.
        /// </summary>
        private readonly VersionGroupService VersionGroupService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AbilityService(
            IDataStoreSource<AbilityEntry> dataStoreSource,
            IPokeAPI pokeApi,
            AbilityCacheService abilityCacheService,
            VersionGroupService versionGroupService,
            ILogger<AbilityService> logger) : base(dataStoreSource, pokeApi, abilityCacheService, logger)
        {
            VersionGroupService = versionGroupService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns an ability entry for the given ability.
        /// </summary>
        protected async override Task<AbilityEntry> ConvertToEntry(Ability ability)
        {
            var displayNames = ability.Names.Localise();
            var flavourTextEntries = await GetFlavourTextEntries(ability);

            return new AbilityEntry
            {
                Key = ability.Id,
                Name = ability.Name,
                DisplayNames = displayNames.ToList(),
                FlavourTextEntries = flavourTextEntries.ToList()
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all abilities.
        /// </summary>
        public async Task<AbilityEntry[]> GetAll()
        {
            var allAbilities = await UpsertAll();
            return allAbilities.OrderBy(g => g.Id).ToArray();
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Returns flavour text entries for the given ability, indexed by version group ID.
        /// </summary>
        private async Task<IEnumerable<WithId<LocalString[]>>> GetFlavourTextEntries(Ability ability)
        {
            var descriptionsList = new List<WithId<LocalString[]>>();

            if (ability.FlavorTextEntries.Any())
            {
                foreach (var vg in await VersionGroupService.GetAll())
                {
                    var relevantDescriptions = ability.FlavorTextEntries.Where(f => f.VersionGroup.Name == vg.Name);
                    if (relevantDescriptions.Any())
                    {
                        var descriptions = relevantDescriptions.Select(d => new LocalString
                        {
                            Language = d.Language.Name,
                            Value = d.FlavorText
                        });

                        descriptionsList.Add(new WithId<LocalString[]>(vg.VersionGroupId, descriptions.ToArray()));
                    }
                }
            }

            return descriptionsList;
        }

        #endregion
    }
}
