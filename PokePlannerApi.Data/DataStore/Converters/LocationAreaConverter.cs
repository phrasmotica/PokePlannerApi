using System.Linq;
using System.Threading.Tasks;
using PokeApiNet;
using PokePlannerApi.Data.DataStore.Services;
using PokePlannerApi.Data.Extensions;
using PokePlannerApi.Models;

namespace PokePlannerApi.Data.DataStore.Converters
{
    public class LocationAreaConverter : IResourceConverter<LocationArea, LocationAreaEntry>
    {
        private readonly LocationService _locationService;

        public LocationAreaConverter(
            LocationService locationService)
        {
            _locationService = locationService;
        }

        /// <inheritdoc />
        public async Task<LocationAreaEntry> Convert(LocationArea resource)
        {
            var displayNames = resource.Names.Localise();
            var location = await _locationService.Get(resource.Location);

            return new LocationAreaEntry
            {
                LocationAreaId = resource.Id,
                Name = resource.Name,
                DisplayNames = displayNames.ToList(),
                Location = location,
            };
        }
    }
}
