using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class EncounterConditionValueConverter : IResourceConverter<EncounterConditionValue, EncounterConditionValueEntry>
    {
        /// <inheritdoc />
        public Task<EncounterConditionValueEntry> Convert(EncounterConditionValue resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new EncounterConditionValueEntry
            {
                EncounterConditionValueId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList()
            });
        }
    }
}
