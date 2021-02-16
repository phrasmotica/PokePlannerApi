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
            var locationArea = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.LocationAreaId == locationArea.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(locationArea);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<LocationAreaEntry> Get(NamedEntryRef<LocationAreaEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.LocationAreaId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var locationArea = await _pokeApi.Get<LocationArea>(entryRef.Key);
            var newEntry = await _converter.Convert(locationArea);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
