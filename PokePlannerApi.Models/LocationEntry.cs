using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a location in the data store.
    /// </summary>
    public class LocationEntry : NamedApiResourceEntry
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

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public LocationEntry ForEvolutionChain()
        {
            return new LocationEntry
            {
                Key = Key,
                Name = Name,
                DisplayNames = DisplayNames
            };
        }
    }
}
