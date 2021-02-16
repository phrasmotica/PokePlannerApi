using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MoveCategoryConverter : IResourceConverter<MoveCategory, MoveCategoryEntry>
    {
        /// <inheritdoc />
        public Task<MoveCategoryEntry> Convert(MoveCategory resource)
        {
            var displayDescriptions = resource.Descriptions.Localise();

            return Task.FromResult(new MoveCategoryEntry
            {
                MoveCategoryId = resource.Id,
                Name = resource.Name,
                Descriptions = displayDescriptions.ToList(),
            });
        }
    }
}
