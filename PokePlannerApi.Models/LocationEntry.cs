using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a location in the data store.
    /// </summary>
    public class LocationEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the location.
        /// </summary>
        public int LocationId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the location.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the location entry.
        /// </summary>
        public NamedEntryRef<LocationEntry> ToRef()
        {
            return new NamedEntryRef<LocationEntry>
            {
                Key = LocationId,
                Name = Name,
            };
        }
    }
}
