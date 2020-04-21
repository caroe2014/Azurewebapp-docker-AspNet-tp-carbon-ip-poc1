using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IpPocApi.Models
{
    public class PingData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("pingerLocalIp")]
        public string PingerLocalIp { get; set; }
        [JsonProperty("pingLocalIp")]
        public string PingLocalIp { get; set; }
        [JsonProperty("pingRemoteIp")]
        public string PingRemoteIp { get; set; }
        [JsonProperty("bounceLocalIp")]
        public string BounceLocalIp { get; set; }
        [JsonProperty("bounceRemoteIp")]
        public string BounceRemoteIp { get; set; }
        //[JsonProperty("remainingIterations")]
        //public int RemainingIterations { get; set; }
        //[JsonProperty("requestedIterations")]
        //public int RequestedIterations { get; set; }
        [JsonProperty("targetUrl")]
        public string TargetUrl { get; set; }
        [JsonProperty("pingUrl")]
        public string PingUrl { get; set; }
        [JsonProperty("bounceUrl")]
        public string BounceUrl { get; set; }


    }
}
