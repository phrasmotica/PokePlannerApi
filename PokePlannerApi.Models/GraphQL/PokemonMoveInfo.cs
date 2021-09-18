using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class PokemonMoveInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("learn_method")]
        public LearnMethodInfo LearnMethod { get; set; }

        [JsonProperty("move")]
        public MoveInfo Move { get; set; }
    }

    public class MoveInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("power")]
        public int? Power { get; set; }

        [JsonProperty("accuracy")]
        public int? Accuracy { get; set; }

        [JsonProperty("pp")]
        public int? Pp { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("damage_class")]
        public MoveDamageClassInfo DamageClass { get; set; }

        [JsonProperty("flavor_texts")]
        public List<FlavorTextInfo> FlavorTexts { get; set; }

        [JsonProperty("meta")]
        public List<MoveMetaInfo> Meta { get; set; }

        [JsonProperty("names")]
        public List<MoveNames> Names { get; set; }

        [JsonProperty("target")]
        public MoveTargetInfo Target { get; set; }

        [JsonProperty("type")]
        public MoveTypeInfo Type { get; set; }
    }

    public class MoveDamageClassInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MoveMetaInfo
    {
        [JsonProperty("category")]
        public Category Category { get; set; }
    }

    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MoveNames
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MoveTargetInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class LearnMethodInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public List<LearnMethodNames> Names { get; set; }
    }

    public class LearnMethodNames
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class MoveTypeInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
