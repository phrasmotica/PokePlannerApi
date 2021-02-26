using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an item in the data store.
    /// </summary>
    public class ItemEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the item.
        /// </summary>
        [Required]
        public int ItemId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the item.
        /// </summary>
        [Required]
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public ItemEntry ForEvolutionChain()
        {
            return new ItemEntry
            {
                ItemId = ItemId,
                Name = Name,
                DisplayNames = DisplayNames
            };
        }
    }

    /// <summary>
    /// Item info plus the chance that some wild Pokemon is holding it in some version.
    /// </summary>
    public class VersionHeldItemContext : ItemEntry
    {
        /// <summary>
        /// Gets or sets the rarity of the held item.
        /// </summary>
        [Required]
        public int Rarity { get; set; }

        /// <summary>
        /// Converts an ability entry into an ability context instance.
        /// </summary>
        public static VersionHeldItemContext From(ItemEntry e)
        {
            return new VersionHeldItemContext
            {
                ItemId = e.ItemId,
                Name = e.Name,
                DisplayNames = e.DisplayNames
            };
        }
    }
}
