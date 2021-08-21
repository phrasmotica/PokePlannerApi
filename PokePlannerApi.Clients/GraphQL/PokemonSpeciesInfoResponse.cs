using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models;

namespace PokePlannerApi.Clients.GraphQL
{
    public class PokemonSpeciesInfoResponse
    {
        [JsonProperty("species_info")]
        public List<PokemonSpeciesInfo> SpeciesInfo { get; set; }
    }
}
