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
        public int LocationId => Key;

        /// <summary>
        /// Gets or sets the display names of the location.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

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
