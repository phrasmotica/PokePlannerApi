using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class GenerationConverter : IResourceConverter<Generation, GenerationEntry>
    {
        /// <inheritdoc />
        public Task<GenerationEntry> Convert(Generation resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new GenerationEntry
            {
                GenerationId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList()
            });
        }
    }
}
