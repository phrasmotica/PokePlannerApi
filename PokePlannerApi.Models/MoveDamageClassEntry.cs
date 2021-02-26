using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move damage class in the data store.
    /// </summary>
    public class MoveDamageClassEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move damage class.
        /// </summary>
        [Required]
        public int MoveDamageClassId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move damage class.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }
    }
}
