using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move learn method in the data store.
    /// </summary>
    public class MoveLearnMethodEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move learn method.
        /// </summary>
        public int MoveLearnMethodId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move learn method.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Gets or sets the descriptions of the move learn method.
        /// </summary>
        public List<LocalString> Descriptions { get; set; }

        /// <summary>
        /// Returns a reference to the move learn method entry.
        /// </summary>
        public EntryRef<MoveLearnMethodEntry> ToRef()
        {
            return new EntryRef<MoveLearnMethodEntry>
            {
                Key = MoveLearnMethodId,
                Name = Name,
            };
        }
    }
}
