using System.ComponentModel.DataAnnotations;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Represents some data associated with a numeric ID.
    /// </summary>
    public class WithId<T>
    {
        /// <summary>
        /// Gets or set the ID.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [Required]
        public T Data { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public WithId(int id, T data) => (Id, Data) = (id, data);
    }
}
