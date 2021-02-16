using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class ItemConverter : IResourceConverter<Item, ItemEntry>
    {
        /// <inheritdoc />
        public Task<ItemEntry> Convert(Item resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new ItemEntry
            {
                ItemId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList()
            });
        }
    }
}
