using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokePlannerApi.Models.GraphQL
{
    public class LocationInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public List<LocationName> Names { get; set; }

        [JsonProperty("location_areas")]
        public List<LocationAreaInfo> LocationAreas { get; set; }
    }

    public class LocationName
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class LocationAreaInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public List<LocationAreaName> Names { get; set; }
    }

    public class LocationAreaName
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
