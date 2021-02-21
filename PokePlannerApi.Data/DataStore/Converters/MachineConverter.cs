using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MachineConverter : IResourceConverter<Machine, MachineEntry>
    {
        private readonly ItemService _itemService;

        public MachineConverter(ItemService itemService)
        {
            _itemService = itemService;
        }

        /// <inheritdoc />
        public async Task<MachineEntry> Convert(Machine resource)
        {
            var item = await _itemService.Get(resource.Item);

            return new MachineEntry
            {
                MachineId = resource.Id,
                Name = resource.Id.ToString(),
                Item = item.ToRef(),
            };
        }
    }
}
