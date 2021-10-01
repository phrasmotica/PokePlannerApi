using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class VersionGroupInfoResponse
    {
        [JsonProperty("version_group_info")]
        public List<VersionGroupInfo> VersionGroupInfo { get; set; }
    }
}
