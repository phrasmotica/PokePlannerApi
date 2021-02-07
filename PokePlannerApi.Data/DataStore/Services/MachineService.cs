using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the machine entries in the data store.
    /// </summary>
    public class MachineService : ServiceBase<Machine, MachineEntry>
    {
        /// <summary>
        /// The item cache service.
        /// </summary>
        private readonly ItemCacheService ItemCacheService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MachineService(
            IDataStoreSource<MachineEntry> dataStoreSource,
            IPokeAPI pokeApi,
            MachineCacheService machineCacheService,
            ItemCacheService itemCacheService,
            ILogger<MachineService> logger) : base(dataStoreSource, pokeApi, machineCacheService, logger)
        {
            ItemCacheService = itemCacheService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a machine entry for the given machine.
        /// </summary>
        protected override async Task<MachineEntry> ConvertToEntry(Machine machine)
        {
            var item = await ItemCacheService.GetMinimal(machine.Item);

            return new MachineEntry
            {
                Key = machine.Id,
                Item = item
            };
        }

        /// <summary>
        /// Returns the machine required to create an machine entry with the given ID.
        /// </summary>
        protected override async Task<Machine> FetchSource(int key)
        {
            Logger.LogInformation($"Fetching machine source object with ID {key}...");
            return await CacheService.Upsert(key);
        }

        #endregion
    }
}
