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
    /// Service for accessing encounter method entries.
    /// </summary>
    public class EncounterMethodService : INamedEntryService<EncounterMethod, EncounterMethodEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<EncounterMethod, EncounterMethodEntry> _converter;
        private readonly IDataStoreSource<EncounterMethodEntry> _dataSource;

        public EncounterMethodService(
            IPokeApi pokeApi,
            IResourceConverter<EncounterMethod, EncounterMethodEntry> converter,
            IDataStoreSource<EncounterMethodEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<EncounterMethodEntry> Get(NamedApiResource<EncounterMethod> resource)
        {
            var encounterMethod = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EncounterMethodId == encounterMethod.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(encounterMethod);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EncounterMethodEntry> Get(NamedEntryRef<EncounterMethodEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.EncounterMethodId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var ability = await _pokeApi.Get<EncounterMethod>(entryRef.Key);
            var newEntry = await _converter.Convert(ability);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<EncounterMethodEntry[]> Get(IEnumerable<NamedApiResource<EncounterMethod>> resources)
        {
            var entries = new List<EncounterMethodEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }
    }
}
