using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IpPocApi.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IpPocApi
{
    public class Sender : ISender
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<Sender> _logger;

        public Sender(ILogger<Sender> logger, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<PingData> ForwardPing(PingData data, string path)
        {
            var client = _clientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(data);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(path, stringContent);
            response.EnsureSuccessStatusCode();

            var newData = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<PingData>(newData);

            return result;
        }
    }
}
