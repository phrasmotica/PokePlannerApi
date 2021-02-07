using System.Collections.Generic;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents a generation in the data store.
    /// </summary>
    public class GenerationEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the generation.
        /// </summary>
        public int GenerationId => Key;

        /// <summary>
        /// Gets or sets the display names of the generation.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
