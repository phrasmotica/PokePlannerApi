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
    /// Service for accessing version entries.
    /// </summary>
    public class VersionService : INamedEntryService<Version, VersionEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Version, VersionEntry> _converter;
        private readonly IDataStoreSource<VersionEntry> _dataSource;

        public VersionService(
            IPokeApi pokeApi,
            IResourceConverter<Version, VersionEntry> converter,
            IDataStoreSource<VersionEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<VersionEntry> Get(NamedApiResource<Version> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<VersionEntry[]> Get(IEnumerable<NamedApiResource<Version>> resources)
        {
            var entries = new List<VersionEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the version with the given name.
        /// </summary>
        /// <param name="name">The version's name.</param>
        private async Task<VersionEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<Version>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <summary>
        /// Returns all versions.
        /// </summary>
        public async Task<VersionEntry[]> GetAll()
        {
            var resources = await _pokeApi.GetNamedFullPage<Version>();
            return await Get(resources.Results);
        }
    }
}
