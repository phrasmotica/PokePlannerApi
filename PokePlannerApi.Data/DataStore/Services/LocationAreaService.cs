using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokePlannerApi.Data.Cache.Services;
using PokePlannerApi.Data.DataStore.Abstractions;
using PokePlannerApi.Data.DataStore.Models;
using PokePlannerApi.Data.Extensions;

namespace PokePlannerApi.Data.DataStore.Services
{
    /// <summary>
    /// Service for managing the location area entries in the data store.
    /// </summary>
    public class LocationAreaService : NamedApiResourceServiceBase<LocationArea, LocationAreaEntry>
    {
        /// <summary>
        /// The locations service.
        /// </summary>
        private readonly LocationService LocationsService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocationAreaService(
            IDataStoreSource<LocationAreaEntry> dataStoreSource,
            IPokeAPI pokeApi,
            LocationAreaCacheService locationAreaCacheService,
            LocationService locationsService,
            ILogger<LocationAreaService> logger) : base(dataStoreSource, pokeApi, locationAreaCacheService, logger)
        {
            LocationsService = locationsService;
        }

        #region Entry conversion methods

        /// <summary>
        /// Returns a location area entry for the given location area.
        /// </summary>
        protected override async Task<LocationAreaEntry> ConvertToEntry(LocationArea locationArea)
        {
            var displayNames = locationArea.Names.Localise();
            var location = await GetLocation(locationArea);

            return new LocationAreaEntry
            {
                Key = locationArea.Id,
                Name = locationArea.Name,
                DisplayNames = displayNames.ToList(),
                Location = new Location
                {
                    Id = location.LocationId,
                    Name = location.Name
                }
            };
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Returns the location of the location area.
        /// </summary>
        private async Task<LocationEntry> GetLocation(LocationArea locationArea)
        {
            return await LocationsService.Upsert(locationArea.Location);
        }

        #endregion
    }
}
