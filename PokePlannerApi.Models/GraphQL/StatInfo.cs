using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class StatInfo
    {
        [JsonProperty("id")]
        public int StatId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_battle_only")]
        public bool IsBattleOnly { get; set; }

        [JsonProperty("pokemon_v2_statnames")]
        public List<StatNamesInfo> StatNamesInfo { get; set; }
    }

    public class StatNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
