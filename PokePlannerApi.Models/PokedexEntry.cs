using System.Collections.Generic;

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
        public int PokedexId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the pokedex.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the pokedex entry.
        /// </summary>
        public EntryRef<PokedexEntry> ToRef()
        {
            return new EntryRef<PokedexEntry>
            {
                Key = PokedexId,
                Name = Name,
            };
        }
    }
}
