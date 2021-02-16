namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents a machine in the data store.
    /// </summary>
    public class MachineEntry : EntryBase
    {
        /// <summary>
        /// Gets the ID of the machine.
        /// </summary>
        public int MachineId
        {
            get => Key;
            set => Key = value;
        }

        /// <summary>
        /// Gets or sets the item this machine represents.
        /// </summary>
        public NamedEntryRef<ItemEntry> Item { get; set; }

        /// <summary>
        /// Returns a reference to the machine entry.
        /// </summary>
        public EntryRef<MachineEntry> ToRef()
        {
            return new EntryRef<MachineEntry>
            {
                Key = MachineId,
            };
        }
    }
}
