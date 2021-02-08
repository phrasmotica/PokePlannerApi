using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move damage class in the data store.
    /// </summary>
    public class MoveDamageClassEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the move damage class.
        /// </summary>
        public int MoveDamageClassId => Key;

        /// <summary>
        /// Gets or sets the display names of the move damage class.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
