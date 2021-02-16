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
    /// Service for accessing evolution chain entries.
    /// </summary>
    public class EvolutionChainService : IEntryService<EvolutionChain, EvolutionChainEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<EvolutionChain, EvolutionChainEntry> _converter;
        private readonly IDataStoreSource<EvolutionChainEntry> _dataSource;

        public EvolutionChainService(
            IPokeApi pokeApi,
            IResourceConverter<EvolutionChain, EvolutionChainEntry> converter,
            IDataStoreSource<EvolutionChainEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<EvolutionChainEntry> Get(ApiResource<EvolutionChain> resource)
        {
            var evolutionChain = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EvolutionChainId == evolutionChain.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(evolutionChain);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EvolutionChainEntry> Get(EntryRef<EvolutionChainEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EvolutionChainId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var evolutionChain = await _pokeApi.Get<EvolutionChain>(entryRef.Key);
            var newEntry = await _converter.Convert(evolutionChain);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EvolutionChainEntry[]> Get(IEnumerable<ApiResource<EvolutionChain>> resources)
        {
            var entries = new List<EvolutionChainEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the evolution chain for the species with the given ID.
        /// </summary>
        public async Task<EvolutionChainEntry> GetBySpeciesId(int speciesId)
        {
            var species = await _pokeApi.Get<PokemonSpecies>(speciesId);
            return await Get(species.EvolutionChain);
        }
    }
}
