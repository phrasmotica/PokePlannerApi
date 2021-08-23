using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models
{
    public class VersionGroupInfoEntry : EntryBase
    {
        [JsonProperty("version_group_info")]
        public List<VersionGroupInfo> VersionGroupInfo { get; set; }
    }

    public class VersionGroupInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pokedexes")]
        public List<PokedexInfo> Pokedexes { get; set; }
    }
}
