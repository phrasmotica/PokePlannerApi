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
    /// Service for accessing encounter condition value entries.
    /// </summary>
    public class EncounterConditionValueService : INamedEntryService<EncounterConditionValue, EncounterConditionValueEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<EncounterConditionValue, EncounterConditionValueEntry> _converter;
        private readonly IDataStoreSource<EncounterConditionValueEntry> _dataSource;

        public EncounterConditionValueService(
            IPokeApi pokeApi,
            IResourceConverter<EncounterConditionValue, EncounterConditionValueEntry> converter,
            IDataStoreSource<EncounterConditionValueEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<EncounterConditionValueEntry> Get(NamedApiResource<EncounterConditionValue> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<EncounterConditionValueEntry> Get(EntryRef<EncounterConditionValueEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<EncounterConditionValueEntry[]> Get(IEnumerable<NamedApiResource<EncounterConditionValue>> resources)
        {
            var entries = new List<EncounterConditionValueEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the encounter condition value with the given name.
        /// </summary>
        /// <param name="name">The encounter condition value's name.</param>
        private async Task<EncounterConditionValueEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<EncounterConditionValue>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
