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
            return resource is null ? null : await Get(resource.Name);
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

        /// <summary>
        /// Returns the evolution trigger with the given name.
        /// </summary>
        /// <param name="name">The evolution trigger's name.</param>
        private async Task<EvolutionTriggerEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<EvolutionTrigger>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
