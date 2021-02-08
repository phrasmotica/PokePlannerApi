using System.Collections.Generic;
using PokeApiNet;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a location area in the data store.
    /// </summary>
    public class LocationAreaEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the location area.
        /// </summary>
        public int LocationAreaId => Key;

        /// <summary>
        /// Gets or sets the display names of the location area.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the location of the location area.
        /// </summary>
        public Location Location { get; set; }
    }
}
