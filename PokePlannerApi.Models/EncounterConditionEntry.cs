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
        public int EncounterConditionId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the encounter condition.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the possible values of the encounter condition.
        /// </summary>
        public List<NamedEntryRef<EncounterConditionValueEntry>> Values { get; set; }

        /// <summary>
        /// Returns a reference to the encounter condition entry.
        /// </summary>
        public NamedEntryRef<EncounterConditionEntry> ToRef()
        {
            return new NamedEntryRef<EncounterConditionEntry>
            {
                Key = EncounterConditionId,
                Name = Name,
            };
        }
    }
}
