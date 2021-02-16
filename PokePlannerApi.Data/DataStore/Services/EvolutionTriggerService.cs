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
    /// Service for accessing evolution trigger entries.
    /// </summary>
    public class EvolutionTriggerService : INamedEntryService<EvolutionTrigger, EvolutionTriggerEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<EvolutionTrigger, EvolutionTriggerEntry> _converter;
        private readonly IDataStoreSource<EvolutionTriggerEntry> _dataSource;

        public EvolutionTriggerService(
            IPokeApi pokeApi,
            IResourceConverter<EvolutionTrigger, EvolutionTriggerEntry> converter,
            IDataStoreSource<EvolutionTriggerEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<EvolutionTriggerEntry> Get(NamedApiResource<EvolutionTrigger> resource)
        {
            var evolutionTrigger = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EvolutionTriggerId == evolutionTrigger.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(evolutionTrigger);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EvolutionTriggerEntry> Get(NamedEntryRef<EvolutionTriggerEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EvolutionTriggerId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var evolutionTrigger = await _pokeApi.Get<EvolutionTrigger>(entryRef.Key);
            var newEntry = await _converter.Convert(evolutionTrigger);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EvolutionTriggerEntry[]> Get(IEnumerable<NamedApiResource<EvolutionTrigger>> resources)
        {
            var entries = new List<EvolutionTriggerEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }
    }
}
