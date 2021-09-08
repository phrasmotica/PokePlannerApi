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

        [JsonProperty("capture_rate")]
        public int CaptureRate { get; set; }

        [JsonProperty("names")]
        public List<PokemonSpeciesNamesInfo> Names { get; set; }

        [JsonProperty("pokedexes")]
        public List<PokemonSpeciesPokedexInfo> Pokedexes { get; set; }

        [JsonProperty("flavor_texts")]
        public List<FlavorTextInfo> FlavorTexts { get; set; }

        [JsonProperty("varieties")]
        public List<VarietyInfo> Varieties { get; set; }
    }

    public class PokemonSpeciesNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("genus")]
        public string Genus { get; set; }
    }

    public class PokemonSpeciesPokedexInfo
    {
        [JsonProperty("pokedex_id")]
        public int PokedexId { get; set; }

        [JsonProperty("pokedex_number")]
        public int PokedexNumber { get; set; }
    }

    public class VarietyInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("abilities")]
        public List<VarietyAbilityInfo> Abilities { get; set; }

        [JsonProperty("forms")]
        public List<FormInfo> Forms { get; set; }

        [JsonProperty("past_types")]
        public List<VarietyPastTypeInfo> PastTypes { get; set; }

        [JsonProperty("stats")]
        public List<BaseStatInfo> Stats { get; set; }

        [JsonProperty("types")]
        public List<VarietyTypeInfo> Types { get; set; }
    }

    public class VarietyAbilityInfo
    {
        [JsonProperty("slot")]
        public int Slot { get; set; }

        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        [JsonProperty("ability")]
        public AbilityInfo Ability { get; set; }
    }

    public class AbilityInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("generation_id")]
        public int GenerationId { get; set; }

        [JsonProperty("names")]
        public List<AbilityNamesInfo> Names { get; set; }

        [JsonProperty("flavor_texts")]
        public List<FlavorTextInfo> FlavorTexts { get; set; }
    }

    public class AbilityNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class FlavorTextInfo
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }
    }

    public class FormInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("form_name")]
        public string FormName { get; set; }

        [JsonProperty("is_mega")]
        public bool IsMega { get; set; }

        [JsonProperty("names")]
        public List<FormNamesInfo> Names { get; set; }

        [JsonProperty("types")]
        public List<FormTypeInfo> Types { get; set; }
    }

    public class VarietyPastTypeInfo
    {
        [JsonProperty("generation_id")]
        public int GenerationId { get; set; }

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

    public class FormNamesInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class FormTypeInfo
    {
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
    }

    public class VarietyTypeInfo
    {
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
    }
}
