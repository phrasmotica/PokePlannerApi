using System.Collections.Generic;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents an encounter condition value in the data store.
    /// </summary>
    public class EncounterConditionValueEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the encounter condition value.
        /// </summary>
        public int EncounterConditionValueId => Key;

        /// <summary>
        /// Gets or sets the display names of the encounter condition value.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
