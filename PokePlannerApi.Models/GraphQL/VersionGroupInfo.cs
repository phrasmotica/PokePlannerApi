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

        [JsonProperty("pokemon_v2_pokedexversiongroups")]
        public List<VersionGroupPokedexInfo> VersionGroupPokedexes { get; set; }

        [JsonProperty("regions")]
        public List<VersionGroupRegionInfo> VersionGroupRegions { get; set; }
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

    public class VersionGroupPokedexInfo
    {
        [JsonProperty("pokemon_v2_pokedex")]
        public PokedexInfo Pokedex { get; set; }
    }

    public class VersionGroupRegionInfo
    {
        [JsonProperty("pokemon_v2_region")]
        public RegionInfo Region { get; set; }
    }

    public class RegionInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public List<RegionNamesInfo> Names { get; set; }
    }

    public class RegionNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
