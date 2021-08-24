using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class TypeInfo
    {
        [JsonProperty("id")]
        public int TypeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("generation_id")]
        public int GenerationId { get; set; }

        [JsonProperty("pokemon_v2_typenames")]
        public List<TypeNamesInfo> TypeNamesInfo { get; set; }
    }

    public class TypeNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
