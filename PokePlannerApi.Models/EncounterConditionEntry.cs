using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an encounter condition in the data store.
    /// </summary>
    public class EncounterConditionEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the encounter condition.
        /// </summary>
        public int EncounterConditionId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the encounter condition.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets the possible values of the encounter condition.
        /// </summary>
        public List<EncounterConditionValueEntry> Values { get; set; } = new List<EncounterConditionValueEntry>();
    }
}
