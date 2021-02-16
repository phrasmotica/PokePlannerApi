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
    /// Service for accessing item entries.
    /// </summary>
    public class ItemService : INamedEntryService<Item, ItemEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Item, ItemEntry> _converter;
        private readonly IDataStoreSource<ItemEntry> _dataSource;

        public ItemService(
            IPokeApi pokeApi,
            IResourceConverter<Item, ItemEntry> converter,
            IDataStoreSource<ItemEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<ItemEntry> Get(NamedApiResource<Item> resource)
        {
            var item = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.ItemId == item.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(item);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<ItemEntry> Get(NamedEntryRef<ItemEntry> entryRef)
        {
            var (hasEntry, entry) = await _dataSource.HasOne(e => e.ItemId == entryRef.Key);
            if (hasEntry)
            {
                return entry;
            }

            var item = await _pokeApi.Get<Item>(entryRef.Key);
            var newEntry = await _converter.Convert(item);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<ItemEntry[]> Get(IEnumerable<NamedApiResource<Item>> resources)
        {
            var entries = new List<ItemEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }
    }
}
