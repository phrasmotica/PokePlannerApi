using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing encounters entries.
    /// </summary>
    public class EncountersService
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Pokemon, EncountersEntry> _converter;
        private readonly IDataStoreSource<EncountersEntry> _dataSource;

        public EncountersService(
            IPokeApi pokeApi,
            IResourceConverter<Pokemon, EncountersEntry> converter,
            IDataStoreSource<EncountersEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <summary>
        /// Returns the encounters of the Pokemon with the given ID.
        /// </summary>
        public async Task<EncountersEntry> GetEncounters(int pokemonId)
        {
            var pokemon = await _pokeApi.Get<Pokemon>(pokemonId);

            var existingEntry = await _dataSource.GetOne(e => e.PokemonId == pokemon.Id);
            if (existingEntry is not null)
            {
                return existingEntry;
            }

            var newEntry = await _converter.Convert(pokemon);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
