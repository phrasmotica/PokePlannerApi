namespace PokePlannerApi.Models
{
    /// <summary>
    /// Class representing a reference to a data store entry.
    /// </summary>
    public class EntryRef<TEntry> where TEntry : EntryBase
    {
        /// <summary>
        /// Gets or sets the key of the entry.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        public string Name { get; set; }
    }
}
