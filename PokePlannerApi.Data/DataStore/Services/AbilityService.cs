using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the ability entries in the data store.
    /// </summary>
    public class AbilityService : IResourceConverter<Ability, AbilityEntry>
    {
        private readonly NamedApiResourceServiceBase<Ability, AbilityEntry> _sourceService;
        private readonly VersionGroupService _versionGroupService;
        private readonly ILogger<AbilityService> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AbilityService(
            NamedApiResourceServiceBase<Ability, AbilityEntry> sourceService,
            VersionGroupService versionGroupService,
            ILogger<AbilityService> logger)
        {
            _sourceService = sourceService;
            _versionGroupService = versionGroupService;
            _logger = logger;
        }

        #region Entry conversion methods

        /// <inheritdoc />
        public async Task<AbilityEntry> ConvertToEntry(Ability ability)
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
            var allAbilities = await _sourceService.UpsertAll();
            return allAbilities.OrderBy(g => g.Id).ToArray();
        }

        /// <summary>
        /// Returns the abilities references by the given resource pointers.
        /// </summary>
        public async Task<AbilityEntry[]> Get(IEnumerable<NamedApiResource<Ability>> resources)
        {
            var allAbilities = await _sourceService.UpsertMany(resources);
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
                foreach (var vg in await _versionGroupService.GetAll())
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
