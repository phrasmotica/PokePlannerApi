using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a stat in the data store.
    /// </summary>
    public class StatEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the stat.
        /// </summary>
        [Required]
        public int StatId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets this stat's display names.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets whether this stat is a battle-only stat.
        /// </summary>
        [Required]
        public bool IsBattleOnly { get; set; }
    }
}
