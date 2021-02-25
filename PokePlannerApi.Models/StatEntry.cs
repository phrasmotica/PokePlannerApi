using System.Collections.Generic;

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
        public int StatId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets this stat's display names.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; } = new List<LocalString>();

        /// <summary>
        /// Gets or sets whether this stat is a battle-only stat.
        /// </summary>
        public bool IsBattleOnly { get; set; }
    }
}
