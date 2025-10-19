using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/Campus")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CampusController : ControllerBase
    {
        private readonly CampusService _campusService;
        private readonly ILogger<CampusController> _logger;
        
        public CampusController(CampusService campusService, ILogger<CampusController> logger)
        {
            _campusService = campusService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all campuses.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Campus GetAll called at {Time}", DateTime.UtcNow);

            var campuses = await _campusService.GetAllAsync();

            return Ok(campuses);
        }

        /// <summary>
        /// Gets a campus by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var campus = await _campusService.FindAsync(id);
            if (campus == null)
            {
                return NotFound();
            }

            return Ok(campus);
        }

        /// <summary>
        /// Creates a campus.
        /// </summary>
        /// <param name="campus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Campus campus)
        {
            if (campus == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Campus data.");
            }

            await _campusService.AddAsync(campus);

            return CreatedAtAction(nameof(GetById), new { id = campus.Id }, campus);
        }

        /// <summary>
        /// Updates a campus.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="campus"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Campus campus)
        {
            if (campus == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid campus data.");
            }

            var existingCampus = await _campusService.FindAsync(campus.Id);
            if (existingCampus == null)
            {
                return NotFound();
            }

            if (existingCampus.Timestamp != campus.Timestamp) 
            {
                return BadRequest("Concurrency check failed");
            }

            await _campusService.UpdateAsync(campus);

            return NoContent();
        }

        /// <summary>
        /// Deletes a campus.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingCampus = await _campusService.FindAsync(id);

            if (existingCampus == null)
            {
                return NotFound();
            }

            if (existingCampus.Timestamp != timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }

            await _campusService.DeleteAsync(existingCampus);

            return NoContent();
        }
    }
}
