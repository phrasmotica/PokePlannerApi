using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a generation in the data store.
    /// </summary>
    public class GenerationEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the generation.
        /// </summary>
        public int GenerationId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the generation.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the generation entry.
        /// </summary>
        public NamedEntryRef<GenerationEntry> ToRef()
        {
            return new NamedEntryRef<GenerationEntry>
            {
                Key = GenerationId,
                Name = Name,
            };
        }
    }
}
