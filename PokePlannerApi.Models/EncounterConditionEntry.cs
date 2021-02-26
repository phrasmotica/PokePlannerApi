using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int EncounterConditionId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the encounter condition.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the possible values of the encounter condition.
        /// </summary>
        [Required]
        public List<EncounterConditionValueEntry> Values { get; set; }
    }
}
