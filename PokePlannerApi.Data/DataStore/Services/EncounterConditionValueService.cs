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
            var encounterConditionValue = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EncounterConditionValueId == encounterConditionValue.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(encounterConditionValue);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EncounterConditionValueEntry> Get(NamedEntryRef<EncounterConditionValueEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EncounterConditionValueId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var encounterConditionValue = await _pokeApi.Get<EncounterConditionValue>(entryRef.Key);
            var newEntry = await _converter.Convert(encounterConditionValue);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
