using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MoveConverter : IResourceConverter<Move, MoveEntry>
    {
        /// <inheritdoc />
        public Task<MoveEntry> Convert(Move resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new MoveEntry
            {
                MoveId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
            });
        }
    }
}
