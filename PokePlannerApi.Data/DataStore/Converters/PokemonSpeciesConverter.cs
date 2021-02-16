using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class PokemonSpeciesConverter : IResourceConverter<PokemonSpecies, PokemonSpeciesEntry>
    {
        private readonly EvolutionChainService _evolutionChainService;
        private readonly GenerationService _generationService;
        private readonly PokemonService _pokemonService;
        private readonly VersionGroupService _versionGroupService;
        private readonly VersionService _versionService;

        public PokemonSpeciesConverter(
            EvolutionChainService evolutionChainService,
            GenerationService generationService,
            PokemonService pokemonService,
            VersionGroupService versionGroupService,
            VersionService versionService)
        {
            _evolutionChainService = evolutionChainService;
            _generationService = generationService;
            _pokemonService = pokemonService;
            _versionGroupService = versionGroupService;
            _versionService = versionService;
        }

        /// <inheritdoc />
        public async Task<PokemonSpeciesEntry> Convert(PokemonSpecies resource)
        {
            var primaryVariety = await _pokemonService.Get(resource.Varieties[0].Pokemon);
            var displayNames = resource.Names.Localise();
            var genera = resource.Genera.Localise();
            var flavourTextEntries = await GetFlavourTextEntries(resource);
            var varieties = await GetVarieties(resource);
            var generation = await _generationService.Get(resource.Generation);
            var evolutionChain = await _evolutionChainService.Get(resource.EvolutionChain);
            var validity = await GetValidity(resource);

            return new PokemonSpeciesEntry
            {
                PokemonSpeciesId = resource.Id,
                Name = resource.Name,
                SpriteUrl = primaryVariety.SpriteUrl,
                ShinySpriteUrl = primaryVariety.ShinySpriteUrl,
                DisplayNames = displayNames.ToList(),
                Genera = genera.ToList(),
                FlavourTextEntries = flavourTextEntries.ToList(),
                Types = primaryVariety.Types.ToList(),
                BaseStats = primaryVariety.BaseStats.ToList(),
                Varieties = varieties.ToList(),
                Generation = generation.ToRef(),
                EvolutionChain = evolutionChain.ToRef(),
                Validity = validity.ToList(),
                CatchRate = resource.CaptureRate
            };
        }

        /// <summary>
        /// Returns flavour text entries for the given species, indexed by version ID.
        /// </summary>
        private async Task<IEnumerable<WithId<List<LocalString>>>> GetFlavourTextEntries(PokemonSpecies species)
        {
            var descriptionsList = new List<WithId<List<LocalString>>>();

            if (species.FlavorTextEntries.Any())
            {
                foreach (var v in await _versionService.GetAll())
                {
                    var relevantDescriptions = species.FlavorTextEntries.Where(f => f.Version.Name == v.Name);
                    if (relevantDescriptions.Any())
                    {
                        var descriptions = relevantDescriptions.Select(d => new LocalString
                        {
                            Language = d.Language.Name,
                            Value = d.FlavorText
                        });

                        descriptionsList.Add(new WithId<List<LocalString>>(v.VersionId, descriptions.ToList()));
                    }
                }
            }

            return descriptionsList;
        }

        /// <summary>
        /// Returns the Pokemon that this Pokemon species represents.
        /// </summary>
        private async Task<IEnumerable<NamedEntryRef<PokemonEntry>>> GetVarieties(PokemonSpecies species)
        {
            var varietiesList = new List<NamedEntryRef<PokemonEntry>>();

            foreach (var res in species.Varieties)
            {
                var pokemon = await _pokemonService.Get(res.Pokemon);
                varietiesList.Add(pokemon.ToRef());
            }

            return varietiesList;
        }

        /// <summary>
        /// Returns the IDs of the version groups where the given Pokemon species is valid.
        /// </summary>
        private async Task<IEnumerable<int>> GetValidity(PokemonSpecies pokemonSpecies)
        {
            var versionGroups = await _versionGroupService.GetAll();
            return versionGroups.Where(vg => IsValid(pokemonSpecies, vg))
                                .Select(vg => vg.VersionGroupId);
        }

        /// <summary>
        /// Returns true if the given Pokemon species can be obtained in the given version group.
        /// </summary>
        private static bool IsValid(PokemonSpecies pokemonSpecies, VersionGroupEntry versionGroup)
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
    }
}
