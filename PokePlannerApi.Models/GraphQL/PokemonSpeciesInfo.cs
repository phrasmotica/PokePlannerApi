using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
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

        [JsonProperty("generation_id")]
        public int GenerationId { get; set; }

        [JsonProperty("pokemon_v2_pokemondexnumbers")]
        public List<PokedexInfo> Pokedexes { get; set; }

        [JsonProperty("varieties")]
        public List<VarietyInfo> Varieties { get; set; }

        [JsonProperty("validity")]
        public List<int> Validity { get; set; }
    }

    public class VarietyInfo
    {
        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("pokemon_v2_pokemontypes")]
        public List<VarietyTypeInfo> Types { get; set; }

        [JsonProperty("pokemon_v2_pokemonstats")]
        public List<BaseStatInfo> BaseStats { get; set; }
    }

    public class VarietyTypeInfo
    {
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
    }

    public class BaseStatInfo
    {
        [JsonProperty("stat_id")]
        public int StatId { get; set; }

        [JsonProperty("base_value")]
        public int BaseValue { get; set; }
    }
}
