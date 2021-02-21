using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Data.Util;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class EncountersConverter : IResourceConverter<Pokemon, EncountersEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly EncounterConditionValueService _encounterConditionValueService;
        private readonly EncounterMethodService _encounterMethodService;
        private readonly LocationAreaService _locationAreaService;
        private readonly VersionService _versionService;
        private readonly VersionGroupService _versionGroupService;

        public EncountersConverter(
            IPokeApi pokeApi,
            EncounterConditionValueService encounterConditionValueService,
            EncounterMethodService encounterMethodService,
            LocationAreaService locationAreaService,
            VersionService versionService,
            VersionGroupService versionGroupService)
        {
            _pokeApi = pokeApi;
            _encounterConditionValueService = encounterConditionValueService;
            _encounterMethodService = encounterMethodService;
            _locationAreaService = locationAreaService;
            _versionService = versionService;
            _versionGroupService = versionGroupService;
        }

        /// <inheritdoc />
        public async Task<EncountersEntry> Convert(Pokemon resource)
        {
            var encounters = await GetEncounters(resource);

            return new EncountersEntry
            {
                PokemonId = resource.Id,
                Name = resource.Id.ToString(),
                Encounters = encounters.ToList()
            };
        }

        /// <summary>
        /// Returns the Pokemon's encounters in all version groups.
        /// </summary>
        private async Task<List<WithId<List<EncounterEntry>>>> GetEncounters(Pokemon pokemon)
        {
            var encounterEntriesList = new List<WithId<List<EncounterEntry>>>();

            // enumerate version groups spanned by this Pokemon's encounters
            var encounters = await _pokeApi.GetEncounters(pokemon);
            var versions = await _versionService.Get(encounters.GetDistinctVersions());
            var versionGroups = await _versionGroupService.GetByVersionIds(versions.Select(v => v.VersionId));

            foreach (var vg in versionGroups)
            {
                var relevantEncounters = encounters.Where(e => IsInVersionGroup(e, vg));

                // create entries for relevant encounters
                var encounterEntries = new List<EncounterEntry>();
                foreach (var encounter in relevantEncounters)
                {
                    var locationArea = await _locationAreaService.Get(encounter.LocationArea);
                    var displayNames = await GetDisplayNames(encounter);

                    // filter to relevant version details
                    var relevantVersionDetails = encounter.VersionDetails.Where(vd =>
                    {
                        var versionName = vd.Version.Name;
                        return vg.Versions.Select(v => v.Name).Contains(versionName);
                    });

                    var chances = await GetChances(relevantVersionDetails);
                    var encounterDetails = await GetEncounterDetails(relevantVersionDetails);

                    var encounterEntry = new EncounterEntry
                    {
                        LocationAreaId = locationArea.LocationAreaId,
                        DisplayNames = displayNames.ToList(),
                        Details = encounterDetails.ToList()
                    };

                    encounterEntries.Add(encounterEntry);
                }

                // add encounter entries to list indexed by version group ID
                var entryList = new WithId<List<EncounterEntry>>(vg.VersionGroupId, encounterEntries.ToList());
                encounterEntriesList.Add(entryList);
            }

            return encounterEntriesList;
        }

        /// <summary>
        /// Returns encounter details sorted by method, indexed by version ID, from the given version details.
        /// </summary>
        public async Task<IEnumerable<WithId<List<EncounterMethodDetails>>>> GetEncounterDetails(IEnumerable<VersionEncounterDetail> versionEncounterDetails)
        {
            var entryList = new List<WithId<List<EncounterMethodDetails>>>();

            var versionGroupings = versionEncounterDetails.GroupBy(d => d.Version, new NamedApiResourceComparer<Version>());

            // loop through versions
            foreach (var versionGrouping in versionGroupings)
            {
                var encountersList = versionGrouping.Select(g => g.EncounterDetails);

                var methodDetailsList = new List<EncounterMethodDetails>();

                // loop through list of encounters in each version
                foreach (var encounters in encountersList)
                {
                    var methodDetails = await GetEncounterMethodDetails(encounters);
                    methodDetailsList.AddRange(methodDetails);
                }

                var version = await _versionService.Get(versionGrouping.Key);
                var versionEntry = new WithId<List<EncounterMethodDetails>>(version.VersionId, methodDetailsList.ToList());
                entryList.Add(versionEntry);
            }

            return entryList;
        }

        /// <summary>
        /// Returns encounter details, grouped by method, from the list of encounter objects.
        /// </summary>
        public async Task<IEnumerable<EncounterMethodDetails>> GetEncounterMethodDetails(List<Encounter> encounters)
        {
            var methodDetailsList = new List<EncounterMethodDetails>();

            var methodGroupings = encounters.GroupBy(d => d.Method, new NamedApiResourceComparer<EncounterMethod>());

            // loop through sets of condition values for each method
            foreach (var methodGrouping in methodGroupings)
            {
                var conditionValuesDetailList = new List<ConditionValuesDetail>();

                var conditionGroupings = methodGrouping.GroupBy(
                    g => g.ConditionValues,
                    new ListComparer<NamedApiResource<EncounterConditionValue>>(
                        (x, y) => x.Url == y.Url,
                        x => x.Url.GetHashCode()
                    )
                );

                // loop through encounters for each set of condition values
                foreach (var conditionGrouping in conditionGroupings)
                {
                    var encounterDetails = new List<EncounterDetailEntry>();

                    var firstEntry = conditionGrouping.First();
                    var conditionValueEntries = await _encounterConditionValueService.Get(firstEntry.ConditionValues);

                    // finally convert the encounters to entries
                    foreach (var encounter in conditionGrouping)
                    {
                        var detailEntry = new EncounterDetailEntry
                        {
                            Chance = encounter.Chance,
                            MaxLevel = encounter.MaxLevel,
                            MinLevel = encounter.MinLevel
                        };

                        encounterDetails.Add(detailEntry);
                    }

                    var conditionValuesDetail = new ConditionValuesDetail
                    {
                        ConditionValues = conditionValueEntries.ToList(),
                        EncounterDetails = encounterDetails
                    };

                    conditionValuesDetailList.Add(conditionValuesDetail);
                }

                var method = await _encounterMethodService.Get(methodGrouping.Key);
                var methodDetails = new EncounterMethodDetails
                {
                    Method = method,
                    ConditionValuesDetails = conditionValuesDetailList.ToList()
                };

                methodDetailsList.Add(methodDetails);
            }

            return methodDetailsList;
        }

        /// <summary>
        /// Returns whether the given encounter is present in the given version group.
        /// </summary>
        private static bool IsInVersionGroup(LocationAreaEncounter encounter, VersionGroupEntry versionGroup)
        {
            var versions = versionGroup.Versions.Select(v => v.Name);
            var encounterVersions = encounter.VersionDetails.Select(vd => vd.Version.Name);

            return versions.Intersect(encounterVersions).Any();
        }

        /// <summary>
        /// Returns the display names of the given encounter.
        /// </summary>
        private async Task<IEnumerable<LocalString>> GetDisplayNames(LocationAreaEncounter encounter)
        {
            var locationArea = await _locationAreaService.Get(encounter.LocationArea);
            var locationAreaNames = locationArea.DisplayNames;
            var locationNames = locationArea.Location.DisplayNames;

            // only provide names in locales that have name data for both location and location area
            var availableLocales = locationNames.Select(n => n.Language)
                                                .Intersect(locationAreaNames.Select(n => n.Language));

            var displayNames = availableLocales.Select(l =>
            {
                var name = locationNames.Single(n => n.Language == l).Value;

                var locationAreaName = locationAreaNames.Single(n => n.Language == l).Value;
                if (!string.IsNullOrEmpty(locationAreaName))
                {
                    name += $" ({locationAreaName})";
                }

                return new LocalString
                {
                    Language = l,
                    Value = name
                };
            });

            return displayNames;
        }

        /// <summary>
        /// Returns the chances of the given encounter indexed by version ID.
        /// </summary>
        private async Task<IEnumerable<WithId<int>>> GetChances(IEnumerable<VersionEncounterDetail> encounterDetails)
        {
            var chancesList = new List<WithId<int>>();

            foreach (var vd in encounterDetails)
            {
                var version = await _versionService.Get(vd.Version);
                var chance = new WithId<int>(version.VersionId, vd.MaxChance);
                chancesList.Add(chance);
            }

            return chancesList;
        }
    }
}
