using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class PokedexInfo
    {
        [JsonProperty("id")]
        public int PokedexId { get; set; }

        [JsonProperty("pokemon_v2_pokedexnames")]
        public List<PokedexNamesInfo> Names { get; set; }
    }

    public class PokedexNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
