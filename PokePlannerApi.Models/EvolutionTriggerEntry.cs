using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a evolution trigger in the data store.
    /// </summary>
    public class EvolutionTriggerEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the evolution trigger.
        /// </summary>
        public int EvolutionTriggerId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the evolution trigger.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the evolution trigger entry.
        /// </summary>
        public NamedEntryRef<EvolutionTriggerEntry> ToRef()
        {
            return new NamedEntryRef<EvolutionTriggerEntry>
            {
                Key = EvolutionTriggerId,
                Name = Name,
            };
        }
    }
}
