using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int EncounterConditionValueId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the encounter condition value.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }
    }
}
