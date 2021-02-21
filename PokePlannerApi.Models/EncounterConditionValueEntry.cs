using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an encounter condition value in the data store.
    /// </summary>
    public class EncounterConditionValueEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the encounter condition value.
        /// </summary>
        public int EncounterConditionValueId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the encounter condition value.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the encounter condition value entry.
        /// </summary>
        public EntryRef<EncounterConditionValueEntry> ToRef()
        {
            return new EntryRef<EncounterConditionValueEntry>
            {
                Key = EncounterConditionValueId,
                Name = Name,
            };
        }
    }
}
