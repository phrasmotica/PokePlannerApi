using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using PokeApiNet;

namespace PokePlannerApi.Data.Cache.Models
{
    /// <summary>
    /// Model for a timestamped cache entry.
    /// </summary>
    public class CacheEntry<T> where T : ResourceBase
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public T Resource { get; set; }
    }
}
