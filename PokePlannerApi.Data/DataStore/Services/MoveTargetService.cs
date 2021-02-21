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
    /// Service for accessing move damage class entries.
    /// </summary>
    public class MoveTargetService : INamedEntryService<MoveTarget, MoveTargetEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<MoveTarget, MoveTargetEntry> _converter;
        private readonly IDataStoreSource<MoveTargetEntry> _dataSource;

        public MoveTargetService(
            IPokeApi pokeApi,
            IResourceConverter<MoveTarget, MoveTargetEntry> converter,
            IDataStoreSource<MoveTargetEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<MoveTargetEntry> Get(NamedApiResource<MoveTarget> resource)
        {
            return resource is null ? null : await Get(resource.Name);
        }

        /// <inheritdoc />
        public async Task<MoveTargetEntry> Get(EntryRef<MoveTargetEntry> entryRef)
        {
            return entryRef is null ? null : await Get(entryRef.Name);
        }

        /// <inheritdoc />
        public async Task<MoveTargetEntry[]> Get(IEnumerable<NamedApiResource<MoveTarget>> resources)
        {
            var entries = new List<MoveTargetEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Returns the move target with the given name.
        /// </summary>
        /// <param name="name">The move target's name.</param>
        private async Task<MoveTargetEntry> Get(string name)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.Name == name);
            if (hasEntry)
            {
                return entry;
            }

            var resource = await _pokeApi.Get<MoveTarget>(name);
            var newEntry = await _converter.Convert(resource);
            await _dataSource.Create(newEntry);

            return newEntry;
        }
    }
}
