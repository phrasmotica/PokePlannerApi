using System.Collections.Generic;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents a move target in the data store.
    /// </summary>
    public class MoveTargetEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the move target.
        /// </summary>
        public int MoveTargetId => Key;

        /// <summary>
        /// Gets or sets the display names of the move target.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
