using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class AbilityConverter : IResourceConverter<Ability, AbilityEntry>
    {
        private readonly VersionGroupService _versionGroupService;

        public AbilityConverter(VersionGroupService versionGroupService)
        {
            _versionGroupService = versionGroupService;
        }

        /// <inheritdoc />
        public async Task<AbilityEntry> Convert(Ability resource)
        {
            var displayNames = resource.Names.Localise();
            var flavourTextEntries = await GetFlavourTextEntries(resource);

            return new AbilityEntry
            {
                AbilityId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
                FlavourTextEntries = flavourTextEntries.ToList()
            };
        }

        /// <summary>
        /// Returns flavour text entries for the given ability, indexed by version group ID.
        /// </summary>
        private async Task<IEnumerable<WithId<List<LocalString>>>> GetFlavourTextEntries(Ability ability)
        {
            var descriptionsList = new List<WithId<List<LocalString>>>();

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
                        }).ToList();

                        descriptionsList.Add(new WithId<List<LocalString>>(vg.VersionGroupId, descriptions));
                    }
                }
            }

            return descriptionsList;
        }
    }
}
