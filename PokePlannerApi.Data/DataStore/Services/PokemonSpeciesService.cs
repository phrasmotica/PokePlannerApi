using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;
using PokemonEntry = PokePlannerApi.Models.PokemonEntry;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing Pokemon species entries.
    /// </summary>
    public class PokemonSpeciesService : INamedEntryService<PokemonSpecies, PokemonSpeciesEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<PokemonSpecies, PokemonSpeciesEntry> _converter;
        private readonly IDataStoreSource<PokemonSpeciesEntry> _dataSource;
        private readonly PokemonService _pokemonService;

        public PokemonSpeciesService(
            IPokeApi pokeApi,
            IResourceConverter<PokemonSpecies, PokemonSpeciesEntry> converter,
            IDataStoreSource<PokemonSpeciesEntry> dataSource,
            PokemonService pokemonService)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
            _pokemonService = pokemonService;
        }

        /// <inheritdoc />
        public async Task<PokemonSpeciesEntry> Get(NamedApiResource<PokemonSpecies> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<PokemonSpeciesEntry> Get(NamedEntryRef<PokemonSpeciesEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<PokemonSpeciesEntry[]> Get(IEnumerable<NamedApiResource<PokemonSpecies>> resources)
        {
            var entries = new List<PokemonSpeciesEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the entry for the Pokemon species with the given ID.
        /// </summary>
        /// <param name="speciesId">The Pokemon species' ID.</param>
        public async Task<PokemonSpeciesEntry> Get(int speciesId)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokemonSpeciesId == speciesId);
            if (hasEntry)
            {
                return entry;
            }

            var pokemonSpecies = await _pokeApi.Get<PokemonSpecies>(speciesId);
            var newEntry = await _converter.Convert(pokemonSpecies);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns the Pokemon species with the given name.
        /// </summary>
        /// <param name="name">The Pokemon species' name.</param>
        private async Task<PokemonSpeciesEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<PokemonSpecies>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns all Pokemon species.
        /// </summary>
        public async Task<PokemonSpeciesEntry[]> GetPokemonSpecies()
        {
            var resources = await _pokeApi.GetNamedFullPage<PokemonSpecies>();
            var allSpecies = await Get(resources.Results);
            return allSpecies.OrderBy(s => s.PokemonSpeciesId).ToArray();
        }

        /// <summary>
        /// Returns all Pokemon species up to a limit from an offset.
        /// </summary>
        public async Task<PokemonSpeciesEntry[]> GetPokemonSpecies(int limit, int offset)
        {
            var resources = await _pokeApi.GetNamedPage<PokemonSpecies>(limit, offset);
            var species = await Get(resources.Results);
            return species.OrderBy(s => s.PokemonSpeciesId).ToArray();
        }

        /// <summary>
        /// Returns the varieties of the Pokemon species with the given ID in the version group with
        /// the given ID.
        /// </summary>
        public async Task<PokemonEntry[]> GetPokemonSpeciesVarieties(int speciesId, int versionGroupId)
        {
            var entry = await Get(speciesId);
            return await _pokemonService.Get(entry.Varieties);
        }

        /// <summary>
        /// Returns the forms of each variety of the Pokemon species with the given ID in the
        /// version group with the given ID.
        /// </summary>
        public async Task<IEnumerable<WithId<List<PokemonFormEntry>>>> GetPokemonSpeciesForms(int speciesId, int versionGroupId)
        {
            var formsListList = new List<WithId<List<PokemonFormEntry>>>();

            var speciesEntry = await Get(speciesId);

            foreach (var varietyEntry in speciesEntry.Varieties)
            {
                var formsList = await _pokemonService.GetPokemonForms(varietyEntry.Key, versionGroupId);
                formsListList.Add(new WithId<List<PokemonFormEntry>>(varietyEntry.Key, formsList));
            }

            return formsListList;
        }
    }
}
