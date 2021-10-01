using System.Collections.Generic;
using Newtonsoft.Json;
using PokePlannerApi.Models.GraphQL;

namespace PokePlannerApi.Clients.GraphQL
{
    public class TypeInfoResponse
    {
        [JsonProperty("type_info")]
        public List<TypeInfo> TypeInfo { get; set; }
    }
}
