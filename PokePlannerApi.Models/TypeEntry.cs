using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a type in the data store.
    /// </summary>
    public class TypeEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the type.
        /// </summary>
        public int TypeId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the type's display names.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets whether the type is concrete.
        /// </summary>
        public bool IsConcrete { get; set; }

        /// <summary>
        /// Gets or sets the generation in which the type was introduced.
        /// </summary>
        [Required]
        public GenerationEntry Generation { get; set; }

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public TypeEntry ForEvolutionChain()
        {
            return new TypeEntry
            {
                Key = Key,
                Name = Name,
                DisplayNames = DisplayNames
            };
        }
    }
}
