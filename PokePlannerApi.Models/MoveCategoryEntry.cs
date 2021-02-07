using System.Collections.Generic;

namespace PokePlannerApi.Data.DataStore.Models
{
    /// <summary>
    /// Represents a move category in the data store.
    /// </summary>
    public class MoveCategoryEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the move category.
        /// </summary>
        public int MoveCategoryId => Key;

        /// <summary>
        /// Gets or sets the descriptions of the move category.
        /// </summary>
        public List<LocalString> Descriptions { get; set; }
    }
}
