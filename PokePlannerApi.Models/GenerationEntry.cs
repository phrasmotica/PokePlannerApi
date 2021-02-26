using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a generation in the data store.
    /// </summary>
    public class GenerationEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the generation.
        /// </summary>
        [Required]
        public int GenerationId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the generation.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }
    }
}
