using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace PokePlannerApi.Models
{
    /// <summary>
    /// Base class for data store entries.
    /// </summary>
    public class EntryBase
    {
        /// <summary>
        /// Gets or sets the ID of the entry.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the key of the entry.
        /// </summary>
        protected int Key { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets the time that the entry was created.
        /// </summary>
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
