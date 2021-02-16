using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Converters;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for accessing move learn method entries.
    /// </summary>
    public class MoveLearnMethodService : INamedEntryService<MoveLearnMethod, MoveLearnMethodEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<MoveLearnMethod, MoveLearnMethodEntry> _converter;
        private readonly IDataStoreSource<MoveLearnMethodEntry> _dataSource;

        public MoveLearnMethodService(
            IPokeApi pokeApi,
            IResourceConverter<MoveLearnMethod, MoveLearnMethodEntry> converter,
            IDataStoreSource<MoveLearnMethodEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        public async Task<MoveLearnMethodEntry> Get(NamedApiResource<MoveLearnMethod> resource)
        {
            var moveLearnMethod = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveLearnMethodId == moveLearnMethod.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(moveLearnMethod);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        public async Task<MoveLearnMethodEntry> Get(NamedEntryRef<MoveLearnMethodEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MoveLearnMethodId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var moveLearnMethod = await _pokeApi.Get<MoveLearnMethod>(entryRef.Key);
            var newEntry = await _converter.Convert(moveLearnMethod);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        public async Task<MoveLearnMethodEntry[]> Get(IEnumerable<NamedApiResource<MoveLearnMethod>> resources)
        {
            var entries = new List<MoveLearnMethodEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }
    }
}
