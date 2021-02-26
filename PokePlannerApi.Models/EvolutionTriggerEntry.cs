using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a evolution trigger in the data store.
    /// </summary>
    public class EvolutionTriggerEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the evolution trigger.
        /// </summary>
        [Required]
        public int EvolutionTriggerId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the evolution trigger.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public EvolutionTriggerEntry ForEvolutionChain()
        {
            return new EvolutionTriggerEntry
            {
                EvolutionTriggerId = EvolutionTriggerId,
                Name = Name,
                DisplayNames = DisplayNames
            };
        }
    }
}
