using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move target in the data store.
    /// </summary>
    public class MoveTargetEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move target.
        /// </summary>
        [Required]
        public int MoveTargetId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move target.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }
    }
}
