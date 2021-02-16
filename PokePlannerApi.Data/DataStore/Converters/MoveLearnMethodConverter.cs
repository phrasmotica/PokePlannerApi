using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MoveLearnMethodConverter : IResourceConverter<MoveLearnMethod, MoveLearnMethodEntry>
    {
        /// <inheritdoc />
        public Task<MoveLearnMethodEntry> Convert(MoveLearnMethod resource)
        {
            var displayNames = resource.Names.Localise();
            var displayDescriptions = resource.Descriptions.Localise();

            return Task.FromResult(new MoveLearnMethodEntry
            {
                MoveLearnMethodId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
                Descriptions = displayDescriptions.ToList(),
            });
        }
    }
}
