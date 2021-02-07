using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Data.Util;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing encounters data.
    /// </summary>
    public class EncountersService : NamedServiceBase<Pokemon, EncountersEntry>
    {
        /// <summary>
        /// The encounter condition value service.
        /// </summary>
        private readonly EncounterConditionValueService EncounterConditionValueService;

        /// <summary>
        /// The encounter method service.
        /// </summary>
        private readonly EncounterMethodService EncounterMethodService;

        /// <summary>
        /// The locations service.
        /// </summary>
        private readonly LocationService LocationsService;

        /// <summary>
        /// The location areas service.
        /// </summary>
        private readonly LocationAreaService LocationAreasService;

        /// <summary>
        /// The versions service.
        /// </summary>
        private readonly VersionService VersionService;

        /// <summary>
        /// The version groups service.
        /// </summary>
        private readonly VersionGroupService VersionGroupsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EncountersService(
            IDataStoreSource<EncountersEntry> dataStoreSource,
            IPokeAPI pokeApi,
            PokemonCacheService pokemonCacheService,
            EncounterConditionValueService encounterConditionValueService,
            EncounterMethodService encounterMethodService,
            LocationService locationsService,
            LocationAreaService locationAreasService,
            VersionService versionsService,
            VersionGroupService versionGroupsService,
            ILogger<EncountersService> logger) : base(dataStoreSource, pokeApi, pokemonCacheService, logger)
        {
            EncounterConditionValueService = encounterConditionValueService;
            EncounterMethodService = encounterMethodService;
            LocationsService = locationsService;
            LocationAreasService = locationAreasService;
            VersionService = versionsService;
            VersionGroupsService = versionGroupsService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns the Pokemon with the given ID.
        /// </summary>
        protected override async Task<Pokemon> FetchSource(int pokemonId)
        {
            Logger.LogInformation($"Fetching Pokemon source object with ID {pokemonId}...");
            return await CacheService.Upsert(pokemonId);
        }

        /// <summary>
        /// Returns a location area encounters entry for the given Pokemon.
        /// </summary>
        protected override async Task<EncountersEntry> ConvertToEntry(Pokemon pokemon)
        {
            var encounters = await GetEncounters(pokemon);

            return new EncountersEntry
            {
                Key = pokemon.Id,
                Encounters = encounters.ToList()
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the encounters of the Pokemon with the given ID.
        /// </summary>
        public async Task<EncountersEntry> GetEncounters(int pokemonId)
        {
            return await Upsert(pokemonId);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the Pokemon's encounters in all version groups.
        /// </summary>
        private async Task<List<WithId<EncounterEntry[]>>> GetEncounters(Pokemon pokemon)
        {
            var encounterEntriesList = new List<WithId<EncounterEntry[]>>();

            // enumerate version groups spanned by this Pokemon's encounters
            // TODO: create encounters cache service
            var encounters = await PokeApi.GetEncounters(pokemon);
            var versions = await VersionService.UpsertMany(encounters.GetDistinctVersions());
            var versionGroups = await VersionGroupsService.UpsertManyByVersionIds(versions.Select(v => v.VersionId));

            foreach (var vg in versionGroups)
            {
                var relevantEncounters = encounters.Where(e => IsInVersionGroup(e, vg));

                // create entries for relevant encounters
                var encounterEntries = new List<EncounterEntry>();
                foreach (var encounter in relevantEncounters)
                {
                    var locationArea = await LocationAreasService.Upsert(encounter.LocationArea);
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
                var entryList = new WithId<EncounterEntry[]>(vg.VersionGroupId, encounterEntries.ToArray());
                encounterEntriesList.Add(entryList);
            }

            return encounterEntriesList;
        }

        /// <summary>
        /// Returns encounter details sorted by method, indexed by version ID, from the given version details.
        /// </summary>
        public async Task<IEnumerable<WithId<EncounterMethodDetails[]>>> GetEncounterDetails(IEnumerable<VersionEncounterDetail> versionEncounterDetails)
        {
            var entryList = new List<WithId<EncounterMethodDetails[]>>();

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

                var version = await VersionService.Upsert(versionGrouping.Key);
                var versionEntry = new WithId<EncounterMethodDetails[]>(version.VersionId, methodDetailsList.ToArray());
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
                    var conditionValueEntries = await EncounterConditionValueService.UpsertMany(firstEntry.ConditionValues);

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

                var method = await EncounterMethodService.Upsert(methodGrouping.Key);
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
        private bool IsInVersionGroup(LocationAreaEncounter encounter, VersionGroupEntry versionGroup)
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
            var locationArea = await LocationAreasService.Upsert(encounter.LocationArea);
            var locationAreaNames = locationArea.DisplayNames;

            var location = await LocationsService.Upsert(locationArea.Location.Id);
            var locationNames = location.DisplayNames;

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
                var version = await VersionService.Upsert(vd.Version);
                var chance = new WithId<int>(version.VersionId, vd.MaxChance);
                chancesList.Add(chance);
            }

            return chancesList;
        }

        #endregion
    }
}
