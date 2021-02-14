using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Clients;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the machine entries in the data store.
    /// </summary>
    public class MachineService : ServiceBase<Machine, MachineEntry>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MachineService(
            IDataStoreSource<MachineEntry> dataStoreSource,
            IPokeAPI pokeApi,
            ILogger<MachineService> logger) : base(dataStoreSource, pokeApi, logger)
        {
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a machine entry for the given machine.
        /// </summary>
        protected override async Task<MachineEntry> ConvertToEntry(Machine machine)
        {
            var item = await _pokeApi.Get(machine.Item);

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
            return await _pokeApi.Get<Machine>(key);
        }

        #endregion
    }
}
