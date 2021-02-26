using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move learn method in the data store.
    /// </summary>
    public class MoveLearnMethodEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move learn method.
        /// </summary>
        [Required]
        public int MoveLearnMethodId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move learn method.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the descriptions of the move learn method.
        /// </summary>
        [Required]
        public List<LocalString> Descriptions { get; set; }
    }
}
