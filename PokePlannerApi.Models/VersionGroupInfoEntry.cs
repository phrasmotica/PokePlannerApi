using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models
{
    public class PokemonSpeciesInfoEntry : EntryBase
    {
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }

        [JsonProperty("generationId")]
        public int GenerationId { get; set; }

        [JsonProperty("species")]
        public List<PokemonSpeciesInfo> Species { get; set; }
    }
}
