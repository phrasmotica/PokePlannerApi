using System.Collections.Generic;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a move target in the data store.
    /// </summary>
    public class MoveTargetEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the move target.
        /// </summary>
        public int MoveTargetId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the display names of the move target.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }

        /// <summary>
        /// Returns a reference to the move target entry.
        /// </summary>
        public EntryRef<MoveTargetEntry> ToRef()
        {
            return new EntryRef<MoveTargetEntry>
            {
                Key = MoveTargetId,
                Name = Name,
            };
        }
    }
}
