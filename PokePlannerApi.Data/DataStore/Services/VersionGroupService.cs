using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing version group entries.
    /// </summary>
    public class VersionGroupService : INamedEntryService<VersionGroup, VersionGroupEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<VersionGroup, VersionGroupEntry> _converter;
        private readonly IDataStoreSource<VersionGroupEntry> _dataSource;

        public VersionGroupService(
            IPokeApi pokeApi,
            IResourceConverter<VersionGroup, VersionGroupEntry> converter,
            IDataStoreSource<VersionGroupEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<VersionGroupEntry> Get(NamedApiResource<VersionGroup> resource)
        {
            var versionGroup = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.VersionGroupId == versionGroup.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(versionGroup);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<VersionGroupEntry> Get(NamedEntryRef<VersionGroupEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.VersionGroupId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var versionGroup = await _pokeApi.Get<VersionGroup>(entryRef.Key);
            var newEntry = await _converter.Convert(versionGroup);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<VersionGroupEntry[]> Get(IEnumerable<NamedApiResource<VersionGroup>> resources)
        {
            var entries = new List<VersionGroupEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns all version groups.
        /// </summary>
        public async Task<VersionGroupEntry[]> GetAll()
        {
            var resources = await _pokeApi.GetNamedFullPage<VersionGroup>();
            return await Get(resources.Results);
        }

        /// <summary>
        /// Returns the index of the oldest version group.
        /// </summary>
        public async Task<int> GetOldestVersionGroupId()
        {
            var entries = await GetAll();
            return entries.Select(vg => vg.VersionGroupId).Min();
        }

        /// <summary>
        /// Returns the index of the newest version group.
        /// </summary>
        public async Task<int> GetNewestVersionGroupId()
        {
            var entries = await GetAll();
            return entries.Select(vg => vg.VersionGroupId).Max();
        }

        /// <summary>
        /// Returns the version groups spanned by the set of version IDs.
        /// </summary>
        public async Task<VersionGroupEntry[]> UpsertManyByVersionIds(IEnumerable<int> versionIds)
        {
            var allVersionGroups = await GetAll();
            var relevantVersionGroups = allVersionGroups.Where(vg =>
            {
                var myVersionIds = vg.Versions.Select(v => v.Key);
                return myVersionIds.Intersect(versionIds).Any();
            });

            return relevantVersionGroups.ToArray();
        }
    }
}
