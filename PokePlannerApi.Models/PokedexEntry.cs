using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a pokedex in the data store.
    /// </summary>
    public class PokedexEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the pokedex.
        /// </summary>
        [Required]
        public int PokedexId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the pokedex.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }
    }
}
