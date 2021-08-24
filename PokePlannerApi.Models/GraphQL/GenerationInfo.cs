using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class GenerationInfo
    {
        [JsonProperty("id")]
        public int GenerationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pokemon_v2_generationnames")]
        public List<GenerationNamesInfo> GenerationNamesInfo { get; set; }
    }

    public class GenerationNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
