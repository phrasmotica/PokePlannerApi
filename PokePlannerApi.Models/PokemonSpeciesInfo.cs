using Newtonsoft.Json;

namespace PokePlannerApi.Models
{
    public class PokemonSpeciesInfo
    {
        [JsonProperty("pokemon_species_id")]
        public int PokemonSpeciesId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("species")]
        public SpeciesInfo Species { get; set; }
    }

    public class SpeciesInfo
    {
        [JsonProperty("order")]
        public int Order { get; set; }
    }
}
