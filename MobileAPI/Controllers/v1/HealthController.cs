using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Authentication;
using System;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Health")]
    [ApiController]
    [ApiVersion("1.0")]
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger) 
        {
            _logger = logger;    
        }

        /// <summary>
        /// Ping the API
        /// </summary>
        /// <returns>200StatusOk with the message "pong"</returns>
        [ApiKey]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping received at {Time}", DateTime.UtcNow);
            return Ok("pong");
        }
    }
}
