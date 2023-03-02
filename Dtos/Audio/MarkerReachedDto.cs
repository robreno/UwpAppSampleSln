using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UwpSample.Dtos
{
    public class MarkerReachedDto
    {
        [JsonProperty("paragraphName")]
        public string ParagraphName { get; set; }
        [JsonProperty("marker")]
        public string Marker { get; set; }
        [JsonProperty("actionRequest")]
        public string ActionRequest { get; set; }
        public MarkerReachedDto()
        { }
        public MarkerReachedDto(string paragraphName, string marker, string request)
        {
            ParagraphName = paragraphName;
            Marker = marker;
            ActionRequest = request;
        }
    }
}
