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
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the possible values of the encounter condition.
        /// </summary>
        public List<EntryRef<EncounterConditionValueEntry>> Values { get; set; }

        /// <summary>
        /// Returns a reference to the encounter condition entry.
        /// </summary>
        public EntryRef<EncounterConditionEntry> ToRef()
        {
            return new EntryRef<EncounterConditionEntry>
            {
                Key = EncounterConditionId,
                Name = Name,
            };
        }
    }
}
