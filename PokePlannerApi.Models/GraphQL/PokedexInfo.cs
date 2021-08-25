using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class PokedexInfo
    {
        [JsonProperty("pokedex_id")]
        public int PokedexId { get; set; }
    }
}
