using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MoveTargetConverter : IResourceConverter<MoveTarget, MoveTargetEntry>
    {
        /// <inheritdoc />
        public Task<MoveTargetEntry> Convert(MoveTarget resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new MoveTargetEntry
            {
                MoveTargetId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
            });
        }
    }
}
