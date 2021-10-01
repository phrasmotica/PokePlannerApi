using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class EncounterMethodConverter : IResourceConverter<EncounterMethod, EncounterMethodEntry>
    {
        /// <inheritdoc />
        public Task<EncounterMethodEntry> Convert(EncounterMethod resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new EncounterMethodEntry
            {
                EncounterMethodId = resource.Id,
                Name = resource.Name,
                Order = resource.Order,
                DisplayNames = displayNames.ToList()
            });
        }
    }
}
