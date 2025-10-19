using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MobileAPI.Models;
using MobileAPI.Services;
using System;
using System.Threading.Tasks;

namespace MobileAPI.Controllers.v1
{
    [Route("v{version:apiVersion}/School")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolService _schoolService;
        private readonly ILogger<SchoolController> _logger;
        
        public SchoolController(SchoolService schoolService, ILogger<SchoolController> logger)
        {
            _schoolService = schoolService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all schools.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("School GetAll called at {Time}", DateTime.UtcNow);
            
            var schools = await _schoolService.GetAllAsync();
            
            return Ok(schools);
        }

        /// <summary>
        /// Gets a school by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var school = await _schoolService.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }
            
            return Ok(school);
        }

        /// <summary>
        /// Creates a school.
        /// </summary>
        /// <param name="school"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] School school)
        {
            if (school == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid School data.");
            }
            
            await _schoolService.AddAsync(school);
            
            return CreatedAtAction(nameof(GetById), new { id = school.Id }, school);
        }

        /// <summary>
        /// Updates a school.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="school"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] School school)
        {
            if (school == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid School data.");
            }
            
            var existingSchool = await _schoolService.FindAsync(school.Id);
            if (existingSchool == null)
            {
                return NotFound();
            }

            if (existingSchool.Timestamp != school.Timestamp)
            {
                return BadRequest("Concurrency check failed");
            }

            await _schoolService.UpdateAsync(school);
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a school.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        [HttpDelete("{id}/{timestamp}")]
        public async Task<IActionResult> Delete(int id, long timestamp)
        {
            var existingSchool = await _schoolService.FindAsync(id);
            if (existingSchool == null)
            {
                return NotFound();
            }

            if (existingSchool.Timestamp != timestamp)
            {
                return BadRequest("Concurrency check failed.");
            }

            await _schoolService.DeleteAsync(existingSchool);
            
            return NoContent();
        }
    }
}