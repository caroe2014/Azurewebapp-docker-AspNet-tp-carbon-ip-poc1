using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IpPocApi.Models;

namespace IpPocApi
{
    public interface ISender
    {
        Task<PingData> ForwardPing(PingData data, string path);
    }
}
