using System.Collections.Generic;
using PokeApiNet;

namespace PokePlannerApi.Data.DataStore.Models
{
    /// <summary>
    /// Represents a version group in the data store.
    /// </summary>
    public class VersionGroupEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the version group.
        /// </summary>
        public int VersionGroupId => Key;

        /// <summary>
        /// Gets or sets the order of the version group.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the display names of the version group.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or set the generation the version group belongs to.
        /// </summary>
        public Generation Generation { get; set; }

        /// <summary>
        /// Gets or set the versions belonging to the version group.
        /// </summary>
        public List<VersionEntry> Versions { get; set; }

        /// <summary>
        /// Gets or set the Pokedexes present in the version group.
        /// </summary>
        public List<Pokedex> Pokedexes { get; set; }
    }
}
