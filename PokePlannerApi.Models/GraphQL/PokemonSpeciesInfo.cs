using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class PokemonSpeciesInfo
    {
        [JsonProperty("id")]
        public int PokemonSpeciesId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("generation_id")]
        public int GenerationId { get; set; }

        [JsonProperty("names")]
        public List<PokemonSpeciesNamesInfo> Names { get; set; }

        [JsonProperty("pokedex_numbers")]
        public List<PokedexInfo> Pokedexes { get; set; }

        [JsonProperty("varieties")]
        public List<VarietyInfo> Varieties { get; set; }

        [JsonProperty("validity")]
        public List<int> Validity { get; set; }
    }

    public class PokemonSpeciesNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class VarietyInfo
    {
        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("types")]
        public List<VarietyTypeInfo> Types { get; set; }

        [JsonProperty("stats")]
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
