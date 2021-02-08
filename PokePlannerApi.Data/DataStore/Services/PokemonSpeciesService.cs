using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the Pokemon species entries in the data store.
    /// </summary>
    public class PokemonSpeciesService : NamedApiResourceServiceBase<PokemonSpecies, PokemonSpeciesEntry>
    {
        /// <summary>
        /// The Pokemon service.
        /// </summary>
        private readonly PokemonService PokemonService;

        /// <summary>
        /// The version group service.
        /// </summary>
        private readonly VersionGroupService VersionGroupService;

        /// <summary>
        /// The version service.
        /// </summary>
        private readonly VersionService VersionService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PokemonSpeciesService(
            IDataStoreSource<PokemonSpeciesEntry> dataStoreSource,
            IPokeAPI pokeApi,
            PokemonService pokemonService,
            VersionGroupService versionGroupService,
            VersionService versionService,
            ILogger<PokemonSpeciesService> logger) : base(dataStoreSource, pokeApi, logger)
        {
            PokemonService = pokemonService;
            VersionGroupService = versionGroupService;
            VersionService = versionService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a Pokemon species entry for the given Pokemon species.
        /// </summary>
        protected override async Task<PokemonSpeciesEntry> ConvertToEntry(PokemonSpecies species)
        {
            var primaryVariety = await PokemonService.Upsert(species.Varieties[0].Pokemon);
            var displayNames = species.Names.Localise();
            var genera = species.Genera.Localise();
            var flavourTextEntries = await GetFlavourTextEntries(species);
            var varieties = await GetVarieties(species);
            var generation = await GetGeneration(species);
            var evolutionChain = await GetEvolutionChain(species);
            var validity = await GetValidity(species);

            return new PokemonSpeciesEntry
            {
                Key = species.Id,
                Name = species.Name,
                SpriteUrl = primaryVariety.SpriteUrl,
                ShinySpriteUrl = primaryVariety.ShinySpriteUrl,
                DisplayNames = displayNames.ToList(),
                Genera = genera.ToList(),
                FlavourTextEntries = flavourTextEntries.ToList(),
                Types = primaryVariety.Types.ToList(),
                BaseStats = primaryVariety.BaseStats.ToList(),
                Varieties = varieties.ToList(),
                Generation = generation,
                EvolutionChain = evolutionChain,
                Validity = validity.ToList(),
                CatchRate = species.CaptureRate
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all Pokemon species.
        /// </summary>
        public async Task<PokemonSpeciesEntry[]> GetPokemonSpecies()
        {
            var allSpecies = await UpsertAll();
            return allSpecies.OrderBy(s => s.SpeciesId).ToArray();
        }

        /// <summary>
        /// Returns all Pokemon species up to a limit from an offset.
        /// </summary>
        public async Task<PokemonSpeciesEntry[]> GetPokemonSpecies(int limit, int offset)
        {
            var resources = await _pokeApi.GetNamedPage<PokemonSpecies>(limit, offset);
            var species = await UpsertMany(resources);
            return species.OrderBy(s => s.SpeciesId).ToArray();
        }

        /// <summary>
        /// Returns the varieties of the Pokemon species with the given ID in the version group with
        /// the given ID.
        /// </summary>
        public async Task<Models.PokemonEntry[]> GetPokemonSpeciesVarieties(int speciesId, int versionGroupId)
        {
            var entry = await Upsert(speciesId);
            var varietyEntries = await PokemonService.UpsertMany(entry.Varieties.Select(v => v.Id));
            return varietyEntries.OrderBy(v => v.PokemonId).ToArray();
        }

        /// <summary>
        /// Returns the forms of each variety of the Pokemon species with the given ID in the
        /// version group with the given ID.
        /// </summary>
        public async Task<IEnumerable<WithId<PokemonFormEntry[]>>> GetPokemonSpeciesForms(int speciesId, int versionGroupId)
        {
            var formsListList = new List<WithId<PokemonFormEntry[]>>();

            var speciesEntry = await Upsert(speciesId);
            var varietyEntries = await PokemonService.UpsertMany(speciesEntry.Varieties.Select(v => v.Id));

            foreach (var varietyEntry in varietyEntries)
            {
                var formsList = await PokemonService.GetPokemonForms(varietyEntry.PokemonId, versionGroupId);
                formsListList.Add(new WithId<PokemonFormEntry[]>(varietyEntry.PokemonId, formsList));
            }

            return formsListList;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns flavour text entries for the given species, indexed by version ID.
        /// </summary>
        private async Task<IEnumerable<WithId<LocalString[]>>> GetFlavourTextEntries(PokemonSpecies species)
        {
            var descriptionsList = new List<WithId<LocalString[]>>();

            if (species.FlavorTextEntries.Any())
            {
                foreach (var v in await VersionService.GetAll())
                {
                    var relevantDescriptions = species.FlavorTextEntries.Where(f => f.Version.Name == v.Name);
                    if (relevantDescriptions.Any())
                    {
                        var descriptions = relevantDescriptions.Select(d => new LocalString
                        {
                            Language = d.Language.Name,
                            Value = d.FlavorText
                        });

                        descriptionsList.Add(new WithId<LocalString[]>(v.VersionId, descriptions.ToArray()));
                    }
                }
            }

            return descriptionsList;
        }

        /// <summary>
        /// Returns the Pokemon that this Pokemon species represents.
        /// </summary>
        private async Task<IEnumerable<Pokemon>> GetVarieties(PokemonSpecies species)
        {
            var varietiesList = new List<Pokemon>();

            foreach (var res in species.Varieties)
            {
                var pokemon = await _pokeApi.Get(res.Pokemon);
                varietiesList.Add(pokemon);
            }

            return varietiesList;
        }

        /// <summary>
        /// Returns the generation in which the given Pokemon species was introduced.
        /// </summary>
        private async Task<Generation> GetGeneration(PokemonSpecies species)
        {
            return await _pokeApi.Get(species.Generation);
        }

        /// <summary>
        /// Returns the evolution chain of the given Pokemon species.
        /// </summary>
        private async Task<EvolutionChain> GetEvolutionChain(PokemonSpecies species)
        {
            return await _pokeApi.Get(species.EvolutionChain);
        }

        /// <summary>
        /// Returns the IDs of the version groups where the given Pokemon species is valid.
        /// </summary>
        private async Task<IEnumerable<int>> GetValidity(PokemonSpecies pokemonSpecies)
        {
            var versionGroups = await VersionGroupService.GetAll();
            return versionGroups.Where(vg => IsValid(pokemonSpecies, vg))
                                .Select(vg => vg.VersionGroupId);
        }

        /// <summary>
        /// Returns true if the given Pokemon species can be obtained in the given version group.
        /// </summary>
        private bool IsValid(PokemonSpecies pokemonSpecies, VersionGroupEntry versionGroup)
        {
            if (!versionGroup.Pokedexes.Any() || !pokemonSpecies.PokedexNumbers.Any())
            {
                // PokeAPI data is incomplete
                return true;
            }

            var versionGroupPokedexes = versionGroup.Pokedexes.Select(p => p.Name);
            var pokemonPokedexes = pokemonSpecies.PokedexNumbers.Select(pn => pn.Pokedex.Name);
            return versionGroupPokedexes.Intersect(pokemonPokedexes).Any();
        }

        #endregion
    }
}
