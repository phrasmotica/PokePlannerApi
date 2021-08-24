using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class VersionGroupInfo
    {
        [JsonProperty("id")]
        public int VersionGroupId { get; set; }

        [JsonProperty("generation_id")]
        public int GenerationId { get; set; }

        [JsonProperty("versions")]
        public List<VersionInfo> VersionInfo { get; set; }

        [JsonProperty("pokedexes")]
        public List<PokedexInfo> Pokedexes { get; set; }
    }

    public class VersionInfo
    {
        [JsonProperty("id")]
        public int VersionId { get; set; }

        [JsonProperty("pokemon_v2_versionnames")]
        public List<VersionNamesInfo> VersionNamesInfo { get; set; }
    }

    public class VersionNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
