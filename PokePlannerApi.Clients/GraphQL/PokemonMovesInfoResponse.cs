using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class PokemonMovesInfoResponse
    {
        [JsonProperty("moves_info")]
        public List<PokemonMoveInfo> MovesInfo { get; set; }
    }
}
