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
            return resource is null ? null : await Get(resource.Name);
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

        /// <summary>
        /// Returns the encounter method with the given name.
        /// </summary>
        /// <param name="name">The encounter method's name.</param>
        private async Task<EncounterMethodEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<EncounterMethod>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
