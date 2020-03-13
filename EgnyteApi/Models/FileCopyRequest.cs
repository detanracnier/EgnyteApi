using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgnyteApi.Models
{
    public class FileCopyRequest
    {
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("destination")]
        public string Destination { get; set; }
    }
}
