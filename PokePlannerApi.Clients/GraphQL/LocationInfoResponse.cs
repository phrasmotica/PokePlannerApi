using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class LocationInfoResponse
    {
        [JsonProperty("location_info")]
        public List<LocationInfo> LocationInfo { get; set; }
    }
}
