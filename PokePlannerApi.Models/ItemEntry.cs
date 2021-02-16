using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents an item in the data store.
    /// </summary>
    public class ItemEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the item.
        /// </summary>
        public int ItemId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the item.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the item entry.
        /// </summary>
        public NamedEntryRef<ItemEntry> ToRef()
        {
            return new NamedEntryRef<ItemEntry>
            {
                Key = ItemId,
                Name = Name,
            };
        }

        /// <summary>
        /// Returns a subset of this entry for use in <see cref="EvolutionChainEntry"/>.
        /// </summary>
        public ItemEntry ForEvolutionChain()
        {
            return new ItemEntry
            {
                Key = Key,
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
