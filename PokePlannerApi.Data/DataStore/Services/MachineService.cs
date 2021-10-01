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
    /// Service for accessing machine entries.
    /// </summary>
    public class MachineService : IEntryService<Machine, MachineEntry>
    {
        private readonly IPokeApi _pokeApi;
        private readonly IResourceConverter<Machine, MachineEntry> _converter;
        private readonly IDataStoreSource<MachineEntry> _dataSource;

        public MachineService(
            IPokeApi pokeApi,
            IResourceConverter<Machine, MachineEntry> converter,
            IDataStoreSource<MachineEntry> dataSource)
        {
            _pokeApi = pokeApi;
            _converter = converter;
            _dataSource = dataSource;
        }

        /// <inheritdoc />
        public async Task<MachineEntry> Get(ApiResource<Machine> resource)
        {
            var machine = await _pokeApi.Get(resource);

            var (hasEntry, entry) = await _dataSource.HasOne(e => e.MachineId == machine.Id);
            if (hasEntry)
            {
                return entry;
            }

            var newEntry = await _converter.Convert(machine);
            await _dataSource.Create(newEntry);

            return newEntry;
        }

        /// <inheritdoc />
        public async Task<MachineEntry[]> Get(IEnumerable<ApiResource<Machine>> resources)
        {
            var entries = new List<MachineEntry>();

            foreach (var v in resources)
            {
                entries.Add(await Get(v));
            }

            return entries.ToArray();
        }
    }
}
