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
            var moveTarget = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveTargetId == moveTarget.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(moveTarget);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<MoveTargetEntry> Get(NamedEntryRef<MoveTargetEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveTargetId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var moveTarget = await _pokeApi.Get<MoveTarget>(entryRef.Key);
            var newEntry = await _converter.Convert(moveTarget);
            await _dataSource.Create(newEntry);

            return newEntry;
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
    }
}
