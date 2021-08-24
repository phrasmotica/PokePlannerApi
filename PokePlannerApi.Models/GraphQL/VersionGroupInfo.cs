using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class VersionGroupInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pokedexes")]
        public List<PokedexInfo> Pokedexes { get; set; }
    }
}
