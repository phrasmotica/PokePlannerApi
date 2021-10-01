using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class EvolutionTriggerConverter : IResourceConverter<EvolutionTrigger, EvolutionTriggerEntry>
    {
        /// <inheritdoc />
        public Task<EvolutionTriggerEntry> Convert(EvolutionTrigger resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new EvolutionTriggerEntry
            {
                EvolutionTriggerId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList()
            });
        }
    }
}
