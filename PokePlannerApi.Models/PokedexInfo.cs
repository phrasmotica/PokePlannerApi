using Newtonsoft.Json;

namespace PokePlannerApi.Models
{
    public class PokedexInfo
    {
        [JsonProperty("pokedex_id")]
        public int PokedexId { get; set; }
    }
}
