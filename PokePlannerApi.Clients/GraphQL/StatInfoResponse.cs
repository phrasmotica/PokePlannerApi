using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class StatInfoResponse
    {
        [JsonProperty("stat_info")]
        public List<StatInfo> StatInfo { get; set; }
    }
}
