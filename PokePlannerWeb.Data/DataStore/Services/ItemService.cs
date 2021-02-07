using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerWeb.Data.Cache.Services;
using PokePlannerWeb.Data.DataStore.Abstractions;
using PokePlannerWeb.Data.DataStore.Models;
using PokePlannerWeb.Data.Extensions;

namespace PokePlannerWeb.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the item entries in the data store.
    /// </summary>
    public class ItemService : NamedApiResourceServiceBase<Item, ItemEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ItemService(
            IDataStoreSource<ItemEntry> dataStoreSource,
            IPokeAPI pokeApi,
            ItemCacheService generationCacheService,
            ILogger<ItemService> logger) : base(dataStoreSource, pokeApi, generationCacheService, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a item entry for the given item.
        /// </summary>
        protected override Task<ItemEntry> ConvertToEntry(Item item)
        {
            var displayNames = item.Names.Localise();

            return Task.FromResult(new ItemEntry
            {
                Key = item.Id,
                Name = item.Name,
                DisplayNames = displayNames.ToList()
            });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns all items.
        /// </summary>
        public async Task<ItemEntry[]> GetAll()
        {
            var allItems = await UpsertAll();
            return allItems.OrderBy(g => g.Id).ToArray();
        }

        #endregion
    }
}
