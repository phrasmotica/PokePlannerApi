using System.Collections.Generic;

namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents a version in the data store.
    /// </summary>
    public class VersionEntry : NamedApiResourceEntry
    {
        /// <summary>
        /// Gets the ID of the version.
        /// </summary>
        public int VersionId => Key;

        /// <summary>
        /// Gets or sets the display names of the version.
        /// </summary>
        public List<LocalString> DisplayNames { get; set; }
    }
}
