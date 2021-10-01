using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class PokemonSpeciesInfoResponse
    {
        [JsonProperty("species_info")]
        public List<PokemonSpeciesInfo> SpeciesInfo { get; set; }
    }
}
