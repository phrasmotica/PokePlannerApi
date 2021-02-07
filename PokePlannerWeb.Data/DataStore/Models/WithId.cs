namespace PokePlannerWeb.Data.DataStore.Models
{
    /// <summary>
    /// Represents some data associated with a numeric ID.
    /// </summary>
    public class WithId<T>
    {
        /// <summary>
        /// Gets or set the ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public WithId(int id, T data) => (Id, Data) = (id, data);
    }
}
