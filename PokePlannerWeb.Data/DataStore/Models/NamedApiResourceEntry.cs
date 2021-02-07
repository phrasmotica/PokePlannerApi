namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Base class for named API resource data store entries.
    /// </summary>
    public class NamedApiResourceEntry : EntryBase
    {
        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        public string Name { get; set; }
    }
}
