using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class LocationConverter : IResourceConverter<Location, LocationEntry>
    {
        /// <inheritdoc />
        public Task<LocationEntry> Convert(Location resource)
        {
            var displayNames = resource.Names.Localise();

            return Task.FromResult(new LocationEntry
            {
                LocationId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList()
            });
        }
    }
}
