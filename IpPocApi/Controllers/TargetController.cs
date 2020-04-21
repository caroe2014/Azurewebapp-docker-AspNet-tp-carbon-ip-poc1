using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IpPocApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IpPocApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetController : ControllerBase
    {
        private readonly ILogger<TargetController> _logger;
        private readonly IActionContextAccessor _accessor;
        private readonly ISender _sender;

        public TargetController(ILogger<TargetController> logger, IActionContextAccessor accessor, ISender sender)
        {
            _logger = logger;
            _accessor = accessor;
            _sender = sender;
        }

        [HttpPost("ping")]
        public async Task<IActionResult> Ping([FromBody] PingData data)
        {
            //data.RemainingIterations = data.RequestedIterations;

            var json = JsonConvert.SerializeObject(data);
            _logger.LogInformation($"PING: {json}");

            var localIp = _accessor.ActionContext.HttpContext.Connection.LocalIpAddress.ToString();
            var remoteIp = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
            var protocol = _accessor.ActionContext.HttpContext.Request.IsHttps ? "https" : "http";
            var pingUrl = $"{protocol}://{_accessor.ActionContext.HttpContext.Request.Host}/{_accessor.ActionContext.HttpContext.Request.Path}";
            

            _logger.LogInformation($"Connection.LocalIpAddress: {localIp}");
            _logger.LogInformation($"Connection.RemoteIpAddress: {remoteIp}");

            PingData result = null;

            var nextData = new PingData
            {
                Id = Guid.NewGuid().ToString(),
                PingerLocalIp = data.PingerLocalIp,
                PingLocalIp = localIp,
                PingRemoteIp = remoteIp,
                TargetUrl = data.TargetUrl,
                PingUrl = pingUrl
            };


            try
            {
                result = await _sender.ForwardPing(nextData, nextData.TargetUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(result);
        }

        [HttpPost("bounce")]
        public async Task<IActionResult> Bounce([FromBody] PingData data)
        {
            var json = JsonConvert.SerializeObject(data);
            _logger.LogInformation($"BOUNCE: {json}");

            var localIp = _accessor.ActionContext.HttpContext.Connection.LocalIpAddress.ToString();
            var remoteIp = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"Connection.LocalIpAddress: {localIp}");
            _logger.LogInformation($"Connection.RemoteIpAddress: {remoteIp}");

            _logger.LogInformation($"Bouncing Request Id: {data.Id}");
            //_logger.LogInformation($"  => RemainingIterations {data.RemainingIterations} of {data.RequestedIterations}");
            _logger.LogInformation($"  => Target: {data.TargetUrl}");

            var protocol = _accessor.ActionContext.HttpContext.Request.IsHttps ? "https" : "http";
            var bounceUrl = $"{protocol}://{_accessor.ActionContext.HttpContext.Request.Host}/{_accessor.ActionContext.HttpContext.Request.Path}";


            PingData result = new PingData
            {
                Id = data.Id,
                BounceLocalIp = localIp,
                BounceRemoteIp = remoteIp,
                PingRemoteIp = data.PingRemoteIp,
                PingLocalIp = data.PingLocalIp,
                PingerLocalIp = data.PingerLocalIp,
                TargetUrl = data.TargetUrl,
                PingUrl = data.PingUrl,
                BounceUrl = bounceUrl
            };

            _logger.LogInformation($"{data.Id} Bounced!");

            //var result = new IpPocResult()
            //{
            //    SenderIp = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString(),
            //    SenderUrl = _accessor.ActionContext.HttpContext.Request.Headers["Referer"].ToString(),
            //    ReceiverIp = _accessor.ActionContext.HttpContext.Connection.LocalIpAddress.ToString(),
            //    ReceiverUrl = data.TargetUrl
            //};

            //if (dataCopy.RemainingIterations < 1)
            //{
            //    _logger.LogInformation($"The bouncing ends here.");
            //}
            //else
            //{
            //    var protocol = _accessor.ActionContext.HttpContext.Request.IsHttps ? "https" : "http";
            //    var destPath = $"{protocol}://{_accessor.ActionContext.HttpContext.Request.Host}/target/bounce";
            //    try
            //    {
            //        result = await _sender.ForwardPing(dataCopy, dataCopy.TargetUrl);
            //        _logger.LogInformation($"Send Complete with status {response.StatusCode}");
            //    }
            //    catch (Exception e)
            //    {
            //        _logger.LogError(e.Message, e);
            //        return StatusCode(StatusCodes.Status500InternalServerError);
            //    }
            //}
            return Ok(result);
        }
    }
}