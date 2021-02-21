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
    /// Service for accessing location area entries.
    /// </summary>
    public class LocationAreaService : INamedEntryService<LocationArea, LocationAreaEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<LocationArea, LocationAreaEntry> _converter;
        private readonly IDataStoreSource<LocationAreaEntry> _dataSource;

        public LocationAreaService(
            IPokeApi pokeApi,
            IResourceConverter<LocationArea, LocationAreaEntry> converter,
            IDataStoreSource<LocationAreaEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<LocationAreaEntry> Get(NamedApiResource<LocationArea> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<LocationAreaEntry> Get(EntryRef<LocationAreaEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<LocationAreaEntry[]> Get(IEnumerable<NamedApiResource<LocationArea>> resources)
        {
            var entries = new List<LocationAreaEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the location area with the given name.
        /// </summary>
        /// <param name="name">The location area's name.</param>
        private async Task<LocationAreaEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<LocationArea>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
