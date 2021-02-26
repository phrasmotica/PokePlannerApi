using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an encounter method in the data store.
    /// </summary>
    public class EncounterMethodEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the encounter method.
        /// </summary>
        [Required]
        public int EncounterMethodId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the order of the encounter method.
        /// </summary>
        [Required]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the display names of the encounter method.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }
    }
}
