using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an encounter condition in the data store.
    /// </summary>
    public class EncounterConditionEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the encounter condition.
        /// </summary>
        public int EncounterConditionId => Key;

        /// <summary>
        /// Gets or sets the display names of the encounter condition.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the possible values of the encounter condition.
        /// </summary>
        public List<EncounterConditionValueEntry> Values { get; set; }
    }
}
