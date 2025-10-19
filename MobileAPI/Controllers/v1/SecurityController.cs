using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Security")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SecurityController : ControllerBase
    {
        private readonly SecurityService _securityService;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(SecurityService securityService, ILogger<SecurityController> logger)
        {
            _securityService = securityService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all security.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Security GetAll called at {Time}", DateTime.UtcNow);
            
            var securitys = await _securityService.GetAllAsync();
            
            return Ok(securitys);
        }

        /// <summary>
        /// Gets a security by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var security = await _securityService.FindAsync(id);
            if (security == null)
            {
                return NotFound();
            }
            
            return Ok(security);
        }

        /// <summary>
        /// Creates a security.
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Security security)
        {
            if (security == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Security data.");
            }
            
            await _securityService.AddAsync(security);
            
            return CreatedAtAction(nameof(GetById), new { id = security.Id }, security);
        }

        /// <summary>
        /// Updates a security.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="security"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Security security)
        {
            if (security == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Security data.");
            }
            
            var existingSecurity = await _securityService.FindAsync(security.Id);
            if (existingSecurity == null)
            {
                return NotFound();
            }
            
            await _securityService.UpdateAsync(security);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a security.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingSecurity = await _securityService.FindAsync(id);
            if (existingSecurity == null)
            {
                return NotFound();
            }

            if (existingSecurity.Timestamp != timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }
            
            await _securityService.DeleteAsync(existingSecurity);
            
            return NoContent();
        }
    }
}