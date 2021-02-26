using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int VersionGroupId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the order of the version group.
        /// </summary>
        [Required]
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the display names of the version group.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the generation the version group belongs to.
        /// </summary>
        [Required]
        public GenerationEntry Generation { get; set; }

        /// <summary>
        /// Gets or sets the versions belonging to the version group.
        /// </summary>
        [Required]
        public List<VersionEntry> Versions { get; set; }

        /// <summary>
        /// Gets or sets the pokedexes present in the version group.
        /// </summary>
        [Required]
        public List<PokedexEntry> Pokedexes { get; set; }
    }
}
