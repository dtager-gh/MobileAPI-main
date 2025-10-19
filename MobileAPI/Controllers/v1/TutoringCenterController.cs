using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/TutoringCenter")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TutoringCenterController : ControllerBase
    {
        private readonly TutoringCenterService _tutoringCenterService;
        private readonly ILogger<TutoringCenterController> _logger;
        
        public TutoringCenterController(TutoringCenterService tutoringCenterService, ILogger<TutoringCenterController> logger)
        {
            _tutoringCenterService = tutoringCenterService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all tutoring centers.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll called at {Time}", DateTime.UtcNow);
            
            var tutoringCenters = await _tutoringCenterService.GetAllAsync();
            
            return Ok(tutoringCenters);
        }

        /// <summary>
        /// Gets a tutoring center by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tutoringCenter = await _tutoringCenterService.FindAsync(id);
            if (tutoringCenter == null)
            {
                return NotFound();
            }
            
            return Ok(tutoringCenter);
        }

        /// <summary>
        /// Creates a tutoring center.
        /// </summary>
        /// <param name="tutoringCenter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TutoringCenter tutoringCenter)
        {
            if (tutoringCenter == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid TutoringCenter data.");
            }
            
            await _tutoringCenterService.AddAsync(tutoringCenter);
            
            return CreatedAtAction(nameof(GetById), new { id = tutoringCenter.Id }, tutoringCenter);
        }

        /// <summary>
        /// Updates a tutoring center.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tutoringCenter"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] TutoringCenter tutoringCenter)
        {
            if (tutoringCenter == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid TutoringCenter data.");
            }
            
            var existingTutoringCenter = await _tutoringCenterService.FindAsync(tutoringCenter.Id);
            if (existingTutoringCenter == null)
            {
                return NotFound();
            }
            
            await _tutoringCenterService.UpdateAsync(tutoringCenter);
            
            return NoContent();
        }

        /// <summary>
        /// Delete a tutoring center.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingTutoringCenter = await _tutoringCenterService.FindAsync(id);
            if (existingTutoringCenter == null)
            {
                return NotFound();
            }

            if (existingTutoringCenter.Timestamp != timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }

            await _tutoringCenterService.DeleteAsync(existingTutoringCenter);
            
            return NoContent();
        }
    }
}