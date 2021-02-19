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
            if (resource is null)
            {
                return null;
            }

            var location = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.LocationId == location.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(location);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<LocationEntry> Get(NamedEntryRef<LocationEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.LocationId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var location = await _pokeApi.Get<Location>(entryRef.Key);
            var newEntry = await _converter.Convert(location);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
