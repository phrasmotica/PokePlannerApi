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
            var pokedex = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokedexId == pokedex.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(pokedex);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<PokedexEntry> Get(NamedEntryRef<PokedexEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.PokedexId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var pokedex = await _pokeApi.Get<Pokedex>(entryRef.Key);
            var newEntry = await _converter.Convert(pokedex);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
