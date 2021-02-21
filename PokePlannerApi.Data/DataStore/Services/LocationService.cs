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
    /// Service for accessing location entries.
    /// </summary>
    public class LocationService : INamedEntryService<Location, LocationEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Location, LocationEntry> _converter;
        private readonly IDataStoreSource<LocationEntry> _dataSource;

        public LocationService(
            IPokeApi pokeApi,
            IResourceConverter<Location, LocationEntry> converter,
            IDataStoreSource<LocationEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<LocationEntry> Get(NamedApiResource<Location> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<LocationEntry> Get(EntryRef<LocationEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<LocationEntry[]> Get(IEnumerable<NamedApiResource<Location>> resources)
        {
            var entries = new List<LocationEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the location with the given name.
        /// </summary>
        /// <param name="name">The location's name.</param>
        private async Task<LocationEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Location>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
