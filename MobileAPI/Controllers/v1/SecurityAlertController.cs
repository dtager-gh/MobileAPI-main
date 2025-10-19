using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/SecurityAlert")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SecurityAlertController : ControllerBase
    {
        private readonly SecurityAlertService _securityAlertService;
        private readonly ILogger<SecurityAlertController> _logger;
    
        public SecurityAlertController(SecurityAlertService securityAlertService, ILogger<SecurityAlertController> logger)
        {
            _securityAlertService = securityAlertService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all security alerts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("SecurityAlert GetAll called at {Time}", DateTime.UtcNow);
            
            var securityAlerts = await _securityAlertService.GetAllAsync();
            
            return Ok(securityAlerts);
        }

        /// <summary>
        /// Get a security alert by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var securityAlert = await _securityAlertService.FindAsync(id);
            if (securityAlert == null)
            {
                return NotFound();
            }
            
            return Ok(securityAlert);
        }

        /// <summary>
        /// Creates a security alert.
        /// </summary>
        /// <param name="securityAlert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SecurityAlert securityAlert)
        {
            if (securityAlert == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid SecurityAlert data.");
            }
            
            await _securityAlertService.AddAsync(securityAlert);
            
            return CreatedAtAction(nameof(GetById), new { id = securityAlert.Id }, securityAlert);
        }

        /// <summary>
        /// Updates a security alert.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="securityAlert"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] SecurityAlert securityAlert)
        {
            if (securityAlert == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid SecurityAlert data.");
            }
            
            var existingSecurityAlert = await _securityAlertService.FindAsync(securityAlert.Id);
            if (existingSecurityAlert == null)
            {
                return NotFound();
            }

            if (existingSecurityAlert.Timestamp != securityAlert.Timestamp)
            {
                return BadRequest("Concurrency check failed");
            }
            
            await _securityAlertService.UpdateAsync(securityAlert);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a security alert.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingSecurityAlert = await _securityAlertService.FindAsync(id);
            if (existingSecurityAlert == null)
            {
                return NotFound();
            }

            if (existingSecurityAlert.Timestamp != timestamp) 
            {
                return BadRequest("Concurrency check failed.");
            }

            await _securityAlertService.DeleteAsync(existingSecurityAlert);
            
            return NoContent();
        }
    }
}