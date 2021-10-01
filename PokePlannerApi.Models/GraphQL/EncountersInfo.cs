using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class EncountersInfo
    {
        [JsonProperty("location_area_id")]
        public int LocationAreaId { get; set; }

        [JsonProperty("pokemon_id")]
        public int PokemonId { get; set; }

        [JsonProperty("min_level")]
        public int MinLevel { get; set; }

        [JsonProperty("max_level")]
        public int MaxLevel { get; set; }

        [JsonProperty("version_id")]
        public int VersionId { get; set; }

        [JsonProperty("conditions")]
        public List<Condition> Conditions { get; set; }

        [JsonProperty("encounter_slot")]
        public EncounterSlot EncounterSlot { get; set; }
    }

    public class EncounterSlot
    {
        [JsonProperty("method")]
        public LearnMethod Method { get; set; }

        [JsonProperty("rarity")]
        public int Rarity { get; set; }

        [JsonProperty("slot")]
        public int Slot { get; set; }
    }

    public class LearnMethod
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public List<LearnMethodName> Names { get; set; }
    }

    public class LearnMethodName
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Condition
    {
        [JsonProperty("pokemon_v2_encounterconditionvalue")]
        public EncounterConditionValue EncounterConditionValue { get; set; }
    }

    public class EncounterConditionValue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
