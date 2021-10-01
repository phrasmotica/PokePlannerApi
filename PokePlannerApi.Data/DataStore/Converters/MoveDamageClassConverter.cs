using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class MoveDamageClassConverter : IResourceConverter<MoveDamageClass, MoveDamageClassEntry>
    {
        /// <inheritdoc />
        public Task<MoveDamageClassEntry> Convert(MoveDamageClass resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new MoveDamageClassEntry
            {
                MoveDamageClassId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
            });
        }
    }
}
