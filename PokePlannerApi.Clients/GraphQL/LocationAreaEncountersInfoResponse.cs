using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class LocationAreaEncountersInfoResponse
    {
        [JsonProperty("encounters_info")]
        public List<EncountersInfo> EncountersInfo { get; set; }
    }
}
