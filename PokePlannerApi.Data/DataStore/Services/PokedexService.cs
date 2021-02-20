using System.Collections.Generic;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for access pokedex entries.
    /// </summary>
    public class PokedexService : INamedEntryService<Pokedex, PokedexEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Pokedex, PokedexEntry> _converter;
        private readonly IDataStoreSource<PokedexEntry> _dataSource;

        public PokedexService(
            IPokeApi pokeApi,
            IResourceConverter<Pokedex, PokedexEntry> converter,
            IDataStoreSource<PokedexEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<PokedexEntry> Get(NamedApiResource<Pokedex> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<PokedexEntry> Get(NamedEntryRef<PokedexEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<PokedexEntry[]> Get(IEnumerable<NamedApiResource<Pokedex>> resources)
        {
            var entries = new List<PokedexEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns all pokedexes.
        /// </summary>
        public async Task<PokedexEntry[]> GetAll()
        {
            var resources = await _pokeApi.GetNamedFullPage<Pokedex>();
            return await Get(resources.Results);
        }

        /// <summary>
        /// Returns the pokedex with the given name.
        /// </summary>
        /// <param name="name">The pokedex's name.</param>
        private async Task<PokedexEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Pokedex>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
