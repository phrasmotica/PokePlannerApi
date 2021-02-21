using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a version group in the data store.
    /// </summary>
    public class VersionGroupEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the version group.
        /// </summary>
        public int VersionGroupId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the order of the version group.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the display names of the version group.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the generation the version group belongs to.
        /// </summary>
        public GenerationEntry Generation { get; set; }

        /// <summary>
        /// Gets or sets the versions belonging to the version group.
        /// </summary>
        public List<VersionEntry> Versions { get; set; }

        /// <summary>
        /// Gets or sets the pokedexes present in the version group.
        /// </summary>
        public List<PokedexEntry> Pokedexes { get; set; }
    }
}
