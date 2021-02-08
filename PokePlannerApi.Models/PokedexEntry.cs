using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a pokedex in the data store.
    /// </summary>
    public class PokedexEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the pokedex.
        /// </summary>
        public int PokedexId => Key;

        /// <summary>
        /// Gets or sets the display names of the pokedex.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
