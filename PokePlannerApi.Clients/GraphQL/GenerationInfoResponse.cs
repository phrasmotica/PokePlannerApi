using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class GenerationInfoResponse
    {
        [JsonProperty("generation_info")]
        public List<GenerationInfo> GenerationInfo { get; set; }
    }
}
