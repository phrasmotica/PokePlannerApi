namespace PokePlannerApi.Models
{
    /// <summary>
    /// Class representing a reference to a named data store entry.
    /// </summary>
    public class NamedEntryRef<TEntry> : EntryRef<TEntry> where TEntry : EntryBase
    {
        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        public string Name { get; set; }
    }
}
