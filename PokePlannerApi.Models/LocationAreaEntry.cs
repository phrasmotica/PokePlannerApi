using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a location area in the data store.
    /// </summary>
    public class LocationAreaEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the location area.
        /// </summary>
        public int LocationAreaId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the location area.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets the location of the location area.
        /// </summary>
        public LocationEntry Location { get; set; } = default!;
    }
}
